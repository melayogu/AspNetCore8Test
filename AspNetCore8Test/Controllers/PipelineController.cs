using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Services.GasServices;
using AspNetCore8Test.Models.DTOs.GasDTOs;

namespace AspNetCore8Test.Controllers
{
    public class PipelineController : Controller
    {
        private readonly IPipelineService _pipelineService;

        public PipelineController(IPipelineService pipelineService)
        {
            _pipelineService = pipelineService;
        }

        // GET: Pipeline
        public async Task<IActionResult> Index()
        {
            var pipelines = await _pipelineService.GetAllPipelinesAsync();
            return View(pipelines);
        }

        // GET: Pipeline/Monitoring
        public async Task<IActionResult> Monitoring()
        {
            var pipelines = await _pipelineService.GetAllPipelinesAsync();
            ViewBag.Pipelines = pipelines.ToList();
            
            // 獲取最新的監控數據（每條管線最新的5筆記錄）
            var monitoringData = new List<PipelineMonitoringDto>();
            foreach (var pipeline in pipelines)
            {
                var latestData = await _pipelineService.GetPipelineMonitoringDataAsync(pipeline.Id);
                monitoringData.AddRange(latestData.Take(5));
            }
            
            return View(monitoringData.OrderByDescending(m => m.RecordTime));
        }

        // GET: Pipeline/MonitoringData/5
        public async Task<IActionResult> MonitoringData(int id, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var pipeline = await _pipelineService.GetPipelineByIdAsync(id);
            if (pipeline == null)
            {
                return NotFound();
            }

            // 預設顯示最近24小時的數據
            if (!fromDate.HasValue)
                fromDate = DateTime.Now.AddDays(-1);
            if (!toDate.HasValue)
                toDate = DateTime.Now;

            var monitoringData = await _pipelineService.GetPipelineMonitoringDataAsync(id, fromDate, toDate);
            
            ViewBag.Pipeline = pipeline;
            ViewBag.FromDate = fromDate.Value.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.ToDate = toDate.Value.ToString("yyyy-MM-ddTHH:mm");
            
            return View(monitoringData);
        }

        // GET: Pipeline/Alerts
        public async Task<IActionResult> Alerts(bool activeOnly = true)
        {
            var alerts = await _pipelineService.GetPipelineAlertsAsync(activeOnly);
            ViewBag.ActiveOnly = activeOnly;
            return View(alerts);
        }

        // GET: Pipeline/AlertDetails/5
        public async Task<IActionResult> AlertDetails(int id)
        {
            var alerts = await _pipelineService.GetPipelineAlertsAsync(false);
            var alert = alerts.FirstOrDefault(a => a.Id == id);
            
            if (alert == null)
            {
                return NotFound();
            }
            
            return View(alert);
        }

        // POST: Pipeline/AcknowledgeAlert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcknowledgeAlert(int alertId, string acknowledgedBy)
        {
            if (string.IsNullOrWhiteSpace(acknowledgedBy))
            {
                TempData["ErrorMessage"] = "請輸入確認人員姓名";
                return RedirectToAction(nameof(Alerts));
            }

            var success = await _pipelineService.AcknowledgeAlertAsync(alertId, acknowledgedBy);
            if (success)
            {
                TempData["SuccessMessage"] = "警報已確認";
            }
            else
            {
                TempData["ErrorMessage"] = "確認警報失敗";
            }

            return RedirectToAction(nameof(Alerts));
        }

        // POST: Pipeline/ResolveAlert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolveAlert(int alertId, string resolvedBy, string resolution)
        {
            if (string.IsNullOrWhiteSpace(resolvedBy) || string.IsNullOrWhiteSpace(resolution))
            {
                TempData["ErrorMessage"] = "請輸入處理人員和解決方案";
                return RedirectToAction(nameof(AlertDetails), new { id = alertId });
            }

            var success = await _pipelineService.ResolveAlertAsync(alertId, resolvedBy, resolution);
            if (success)
            {
                TempData["SuccessMessage"] = "警報已解決";
                return RedirectToAction(nameof(Alerts));
            }
            else
            {
                TempData["ErrorMessage"] = "解決警報失敗";
                return RedirectToAction(nameof(AlertDetails), new { id = alertId });
            }
        }

        // GET: Pipeline/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var pipeline = await _pipelineService.GetPipelineByIdAsync(id);
            if (pipeline == null)
            {
                return NotFound();
            }

            // 獲取最新監控數據
            var latestMonitoring = await _pipelineService.GetPipelineMonitoringDataAsync(id);
            ViewBag.LatestMonitoring = latestMonitoring.Take(10);

            // 獲取相關警報
            var alerts = await _pipelineService.GetPipelineAlertsByIdAsync(id, false);
            ViewBag.Alerts = alerts.Take(10);

            return View(pipeline);
        }

        // GET: Pipeline/RealTimeMonitoring
        public async Task<IActionResult> RealTimeMonitoring()
        {
            var pipelines = await _pipelineService.GetAllPipelinesAsync();
            var activePipelines = pipelines.Where(p => p.Status == "Active").ToList();
            
            // 為每條活躍管線獲取最新監控數據
            var realTimeData = new List<PipelineMonitoringDto>();
            foreach (var pipeline in activePipelines)
            {
                var latestData = await _pipelineService.GetPipelineMonitoringDataAsync(pipeline.Id);
                var latest = latestData.FirstOrDefault();
                if (latest != null)
                {
                    realTimeData.Add(latest);
                }
            }
            
            return View(realTimeData);
        }

        // API: Pipeline/GetLatestData/5
        [HttpGet]
        public async Task<IActionResult> GetLatestData(int id)
        {
            var latestData = await _pipelineService.GetPipelineMonitoringDataAsync(id);
            var latest = latestData.FirstOrDefault();
            
            if (latest == null)
            {
                return NotFound();
            }
            
            return Json(latest);
        }

        // API: Pipeline/AddMonitoringData
        [HttpPost]
        public async Task<IActionResult> AddMonitoringData(int pipelineId, decimal pressure, decimal flowRate, decimal temperature, decimal humidity)
        {
            try
            {
                var monitoringData = await _pipelineService.AddMonitoringDataAsync(pipelineId, pressure, flowRate, temperature, humidity);
                return Json(new { success = true, data = monitoringData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}