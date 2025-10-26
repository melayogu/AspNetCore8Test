using AspNetCore8Test.Models.DTOs.LoRaDtos;
using AspNetCore8Test.Services.LoRaServices;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers.LoRaControllers
{
    [ApiController]
    [Route("api/lora-metering")]
    public class LoRaMeteringController : ControllerBase
    {
        private readonly ILoRaMeteringService _service;
        private readonly IValidator<CreateLoRaDeviceDto> _createDeviceValidator;
        private readonly IValidator<UpdateLoRaDeviceDto> _updateDeviceValidator;
        private readonly IValidator<CreateMeterReadingDto> _createReadingValidator;
        private readonly IValidator<AlertActionDto> _alertActionValidator;

        public LoRaMeteringController(
            ILoRaMeteringService service,
            IValidator<CreateLoRaDeviceDto> createDeviceValidator,
            IValidator<UpdateLoRaDeviceDto> updateDeviceValidator,
            IValidator<CreateMeterReadingDto> createReadingValidator,
            IValidator<AlertActionDto> alertActionValidator)
        {
            _service = service;
            _createDeviceValidator = createDeviceValidator;
            _updateDeviceValidator = updateDeviceValidator;
            _createReadingValidator = createReadingValidator;
            _alertActionValidator = alertActionValidator;
        }

        /// <summary>
        /// 取得所有 LoRa 抄表裝置。
        /// </summary>
        [HttpGet("devices")]
        public async Task<ActionResult<IEnumerable<LoRaDeviceDto>>> GetDevices()
        {
            var devices = await _service.GetAllDevicesAsync();
            return Ok(devices);
        }

        /// <summary>
        /// 透過裝置 ID 取得裝置資訊。
        /// </summary>
        [HttpGet("devices/{id:int}")]
        public async Task<ActionResult<LoRaDeviceDto>> GetDeviceById(int id)
        {
            var device = await _service.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound($"找不到 ID 為 {id} 的 LoRa 裝置");
            }

            return Ok(device);
        }

        /// <summary>
        /// 透過 DeviceEUI 查詢裝置。
        /// </summary>
        [HttpGet("devices/by-eui/{deviceEui}")]
        public async Task<ActionResult<LoRaDeviceDto>> GetDeviceByEui(string deviceEui)
        {
            var device = await _service.GetDeviceByEuiAsync(deviceEui);
            if (device == null)
            {
                return NotFound($"找不到 DeviceEUI 為 {deviceEui} 的 LoRa 裝置");
            }

            return Ok(device);
        }

        /// <summary>
        /// 透過表號查詢裝置。
        /// </summary>
        [HttpGet("devices/by-meter/{meterNumber}")]
        public async Task<ActionResult<LoRaDeviceDto>> GetDeviceByMeterNumber(string meterNumber)
        {
            var device = await _service.GetDeviceByMeterNumberAsync(meterNumber);
            if (device == null)
            {
                return NotFound($"找不到表號為 {meterNumber} 的 LoRa 裝置");
            }

            return Ok(device);
        }

        /// <summary>
        /// 新增一個 LoRa 抄表裝置。
        /// </summary>
        [HttpPost("devices")]
        public async Task<ActionResult<LoRaDeviceDto>> CreateDevice([FromBody] CreateLoRaDeviceDto dto)
        {
            var validationResult = await _createDeviceValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return ValidationProblem(ModelState);
            }

            if (await _service.GetDeviceByEuiAsync(dto.DeviceEui) != null)
            {
                return Conflict($"DeviceEUI {dto.DeviceEui} 已存在");
            }

            if (await _service.GetDeviceByMeterNumberAsync(dto.MeterNumber) != null)
            {
                return Conflict($"表號 {dto.MeterNumber} 已存在");
            }

            var device = await _service.CreateDeviceAsync(dto);
            return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
        }

        /// <summary>
        /// 更新 LoRa 抄表裝置資訊。
        /// </summary>
        [HttpPut("devices/{id:int}")]
        public async Task<ActionResult<LoRaDeviceDto>> UpdateDevice(int id, [FromBody] UpdateLoRaDeviceDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("路徑中的 ID 與資料內容不一致");
            }

            var validationResult = await _updateDeviceValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return ValidationProblem(ModelState);
            }

            var device = await _service.UpdateDeviceAsync(id, dto);
            if (device == null)
            {
                return NotFound($"找不到 ID 為 {id} 的 LoRa 裝置");
            }

            return Ok(device);
        }

        /// <summary>
        /// 刪除 LoRa 抄表裝置。
        /// </summary>
        [HttpDelete("devices/{id:int}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var deleted = await _service.DeleteDeviceAsync(id);
            if (!deleted)
            {
                return NotFound($"找不到 ID 為 {id} 的 LoRa 裝置");
            }

            return NoContent();
        }

        /// <summary>
        /// 取得指定裝置的抄表資料。
        /// </summary>
        [HttpGet("devices/{id:int}/readings")]
        public async Task<ActionResult<IEnumerable<MeterReadingDto>>> GetDeviceReadings(
            int id,
            [FromQuery] DateTime? start = null,
            [FromQuery] DateTime? end = null)
        {
            var device = await _service.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound($"找不到 ID 為 {id} 的 LoRa 裝置");
            }

            var readings = await _service.GetDeviceReadingsAsync(id, start, end);
            return Ok(readings);
        }

        /// <summary>
        /// 新增裝置的抄表資料。
        /// </summary>
        [HttpPost("devices/{id:int}/readings")]
        public async Task<ActionResult<MeterReadingDto>> AddDeviceReading(int id, [FromBody] CreateMeterReadingDto dto)
        {
            var validationResult = await _createReadingValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return ValidationProblem(ModelState);
            }

            var reading = await _service.AddDeviceReadingAsync(id, dto);
            if (reading == null)
            {
                return NotFound($"找不到 ID 為 {id} 的 LoRa 裝置");
            }

            return CreatedAtAction(nameof(GetDeviceReadings), new { id }, reading);
        }

        /// <summary>
        /// 取得所有閘道器即時狀態。
        /// </summary>
        [HttpGet("gateways")]
        public async Task<ActionResult<IEnumerable<LoRaGatewayDto>>> GetGateways()
        {
            var gateways = await _service.GetGatewaysAsync();
            return Ok(gateways);
        }

        /// <summary>
        /// 取得系統摘要資訊。
        /// </summary>
        [HttpGet("summary")]
        public async Task<ActionResult<LoRaNetworkSummaryDto>> GetSummary()
        {
            var summary = await _service.GetNetworkSummaryAsync();
            return Ok(summary);
        }

        /// <summary>
        /// 取得所有未結案警報。
        /// </summary>
        [HttpGet("alerts")]
        public async Task<ActionResult<IEnumerable<LoRaAlertDto>>> GetActiveAlerts()
        {
            var alerts = await _service.GetActiveAlertsAsync();
            return Ok(alerts);
        }

        /// <summary>
        /// 取得某個裝置的警報歷史。
        /// </summary>
        [HttpGet("devices/{id:int}/alerts")]
        public async Task<ActionResult<IEnumerable<LoRaAlertDto>>> GetDeviceAlerts(int id)
        {
            var device = await _service.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound($"找不到 ID 為 {id} 的 LoRa 裝置");
            }

            var alerts = await _service.GetDeviceAlertsAsync(id);
            return Ok(alerts);
        }

        /// <summary>
        /// 確認警報。
        /// </summary>
        [HttpPost("alerts/{id:int}/acknowledge")]
        public async Task<ActionResult<LoRaAlertDto>> AcknowledgeAlert(int id, [FromBody] AlertActionDto dto)
        {
            var validationResult = await _alertActionValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return ValidationProblem(ModelState);
            }

            var alert = await _service.AcknowledgeAlertAsync(id, dto.OperatorName);
            if (alert == null)
            {
                return NotFound($"找不到 ID 為 {id} 的警報");
            }

            return Ok(alert);
        }

        /// <summary>
        /// 結案警報。
        /// </summary>
        [HttpPost("alerts/{id:int}/resolve")]
        public async Task<ActionResult<LoRaAlertDto>> ResolveAlert(int id, [FromBody] AlertActionDto dto)
        {
            var validationResult = await _alertActionValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return ValidationProblem(ModelState);
            }

            var alert = await _service.ResolveAlertAsync(id, dto.OperatorName);
            if (alert == null)
            {
                return NotFound($"找不到 ID 為 {id} 的警報");
            }

            return Ok(alert);
        }
    }
}
