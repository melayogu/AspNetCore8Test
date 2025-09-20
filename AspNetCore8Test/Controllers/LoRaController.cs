using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.GasServices;
using AspNetCore8Test.Models.DTOs.GasDTOs;

namespace AspNetCore8Test.Controllers
{
    /// <summary>
    /// LoRa 無線抄表系統的儀表板與管理入口
    /// </summary>
    public class LoRaController : Controller
    {
        private readonly ILoRaMeterService _loRaMeterService;

        public LoRaController(ILoRaMeterService loRaMeterService)
        {
            _loRaMeterService = loRaMeterService;
        }

        // GET: /LoRa
        public async Task<IActionResult> Index()
        {
            var dashboard = await _loRaMeterService.GetDashboardAsync();
            return View(dashboard);
        }

        // GET: /LoRa/Devices
        public async Task<IActionResult> Devices(string status = "all", string sort = "signal")
        {
            var devices = (await _loRaMeterService.GetDevicesAsync(status)).ToList();

            switch (sort?.ToLowerInvariant())
            {
                case "battery":
                    devices = devices.OrderBy(d => d.BatteryLevel).ToList();
                    break;
                case "usage":
                    devices = devices.OrderByDescending(d => d.MonthlyUsage).ToList();
                    break;
                case "reading":
                    devices = devices.OrderByDescending(d => d.LastReadingTime).ToList();
                    break;
                default:
                    devices = devices.OrderByDescending(d => d.SignalStrength).ToList();
                    break;
            }

            ViewBag.StatusFilter = status;
            ViewBag.Sort = sort;

            return View(devices);
        }

        // GET: /LoRa/DeviceDetails/5
        public async Task<IActionResult> DeviceDetails(int id)
        {
            var device = await _loRaMeterService.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            var readings = await _loRaMeterService.GetDeviceReadingsAsync(id, 40);
            var alerts = (await _loRaMeterService.GetAlertsAsync(includeResolved: true))
                .Where(a => a.DeviceId == id)
                .OrderByDescending(a => a.DetectedAt)
                .ToList();

            var viewModel = new LoRaDeviceDetailsViewModel
            {
                Device = device,
                RecentReadings = readings,
                Alerts = alerts
            };

            return View(viewModel);
        }

        // GET: /LoRa/Gateways
        public async Task<IActionResult> Gateways()
        {
            var gateways = await _loRaMeterService.GetGatewaysAsync();
            return View(gateways);
        }

        // GET: /LoRa/Alerts
        public async Task<IActionResult> Alerts(bool includeResolved = false)
        {
            var alerts = await _loRaMeterService.GetAlertsAsync(includeResolved);
            ViewBag.IncludeResolved = includeResolved;
            return View(alerts);
        }

        // GET: /LoRa/Readings
        public async Task<IActionResult> Readings(int? deviceId = null)
        {
            IEnumerable<LoRaReadingDto> readings;
            LoRaDeviceDto? device = null;

            if (deviceId.HasValue)
            {
                device = await _loRaMeterService.GetDeviceByIdAsync(deviceId.Value);
                if (device == null)
                {
                    return NotFound();
                }

                readings = await _loRaMeterService.GetDeviceReadingsAsync(deviceId.Value, 100);
            }
            else
            {
                readings = await _loRaMeterService.GetRecentReadingsAsync(50);
            }

            ViewBag.Device = device;
            return View(readings);
        }

        // POST: /LoRa/AcknowledgeAlert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcknowledgeAlert(int alertId, string acknowledgedBy)
        {
            if (string.IsNullOrWhiteSpace(acknowledgedBy))
            {
                TempData["ErrorMessage"] = "請輸入確認人員姓名";
                return RedirectToAction(nameof(Alerts));
            }

            var success = await _loRaMeterService.AcknowledgeAlertAsync(alertId, acknowledgedBy);
            TempData[success ? "SuccessMessage" : "ErrorMessage"] = success
                ? "警報已確認"
                : "警報確認失敗或已被處理";

            return RedirectToAction(nameof(Alerts));
        }

        // POST: /LoRa/ResolveAlert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolveAlert(int alertId, string resolvedBy)
        {
            if (string.IsNullOrWhiteSpace(resolvedBy))
            {
                TempData["ErrorMessage"] = "請輸入處理人員姓名";
                return RedirectToAction(nameof(Alerts));
            }

            var success = await _loRaMeterService.ResolveAlertAsync(alertId, resolvedBy);
            TempData[success ? "SuccessMessage" : "ErrorMessage"] = success
                ? "警報已設定為已解決"
                : "更新警報狀態失敗";

            return RedirectToAction(nameof(Alerts));
        }
    }
}
