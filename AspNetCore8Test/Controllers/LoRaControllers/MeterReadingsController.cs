using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Services.LoRaServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers.LoRaControllers
{
    [ApiController]
    [Route("api/lora/[controller]")]
    public class MeterReadingsController : ControllerBase
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly ILoRaDeviceService _deviceService;
        private readonly IValidator<CreateMeterReadingDto> _createValidator;

        public MeterReadingsController(
            IMeterReadingService meterReadingService,
            ILoRaDeviceService deviceService,
            IValidator<CreateMeterReadingDto> createValidator)
        {
            _meterReadingService = meterReadingService;
            _deviceService = deviceService;
            _createValidator = createValidator;
        }

        /// <summary>
        /// 取得所有讀值，或依照條件篩選
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterReadingDto>>> GetReadings(
            [FromQuery] string? deviceEui,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            if (!string.IsNullOrWhiteSpace(deviceEui))
            {
                if (!await _deviceService.DeviceEuiExistsAsync(deviceEui))
                {
                    return NotFound($"找不到 Device EUI 為 {deviceEui} 的設備");
                }

                var readings = await _meterReadingService.GetReadingsByDeviceAsync(deviceEui, startDate, endDate);
                return Ok(readings);
            }

            var all = await _meterReadingService.GetAllReadingsAsync();
            return Ok(all);
        }

        /// <summary>
        /// 取得指定設備的所有讀值
        /// </summary>
        [HttpGet("device/{deviceEui}")]
        public async Task<ActionResult<IEnumerable<MeterReadingDto>>> GetDeviceReadings(string deviceEui)
        {
            if (!await _deviceService.DeviceEuiExistsAsync(deviceEui))
            {
                return NotFound($"找不到 Device EUI 為 {deviceEui} 的設備");
            }

            var readings = await _meterReadingService.GetReadingsByDeviceAsync(deviceEui);
            return Ok(readings);
        }

        /// <summary>
        /// 取得指定設備最新的讀值
        /// </summary>
        [HttpGet("device/{deviceEui}/latest")]
        public async Task<ActionResult<MeterReadingDto>> GetLatestReading(string deviceEui)
        {
            if (!await _deviceService.DeviceEuiExistsAsync(deviceEui))
            {
                return NotFound($"找不到 Device EUI 為 {deviceEui} 的設備");
            }

            var reading = await _meterReadingService.GetLatestReadingAsync(deviceEui);
            if (reading == null)
            {
                return NotFound("目前尚未收到任何讀值");
            }

            return Ok(reading);
        }

        /// <summary>
        /// 依日彙總指定設備的用量
        /// </summary>
        [HttpGet("device/{deviceEui}/daily-summary")]
        public async Task<ActionResult<IEnumerable<MeterReadingSummaryDto>>> GetDailySummary(
            string deviceEui,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            if (!await _deviceService.DeviceEuiExistsAsync(deviceEui))
            {
                return NotFound($"找不到 Device EUI 為 {deviceEui} 的設備");
            }

            var summary = await _meterReadingService.GetDailySummaryAsync(deviceEui, startDate, endDate);
            return Ok(summary);
        }

        /// <summary>
        /// 新增設備讀值
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MeterReadingDto>> CreateReading(CreateMeterReadingDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            if (!await _deviceService.DeviceEuiExistsAsync(createDto.DeviceEui))
            {
                return BadRequest($"找不到 Device EUI 為 {createDto.DeviceEui} 的設備");
            }

            var reading = await _meterReadingService.CreateReadingAsync(createDto);
            return CreatedAtAction(nameof(GetLatestReading), new { deviceEui = createDto.DeviceEui }, reading);
        }
    }
}
