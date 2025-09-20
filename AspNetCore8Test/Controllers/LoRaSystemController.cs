using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Services.LoRaServices;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoRaSystemController : ControllerBase
    {
        private readonly ILoRaSystemService _loRaSystemService;

        public LoRaSystemController(ILoRaSystemService loRaSystemService)
        {
            _loRaSystemService = loRaSystemService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<LoRaDashboardStatsDto>> GetStats()
        {
            try
            {
                var stats = await _loRaSystemService.GetDashboardStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入 LoRa 統計資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpGet("gateways")]
        public async Task<ActionResult<IEnumerable<LoRaGatewayDto>>> GetGateways()
        {
            try
            {
                var gateways = await _loRaSystemService.GetGatewaysAsync();
                return Ok(gateways);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入閘道器資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpGet("devices")]
        public async Task<ActionResult<IEnumerable<LoRaDeviceDto>>> GetDevices([FromQuery] string? status = null, [FromQuery] string? search = null, [FromQuery] int? gatewayId = null)
        {
            try
            {
                var devices = await _loRaSystemService.GetDevicesAsync(status, search, gatewayId);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入設備資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpGet("devices/{id}")]
        public async Task<ActionResult<LoRaDeviceDto>> GetDevice(int id)
        {
            try
            {
                var device = await _loRaSystemService.GetDeviceByIdAsync(id);
                if (device == null)
                {
                    return NotFound(new { message = $"找不到編號為 {id} 的設備" });
                }

                return Ok(device);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入設備資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpPost("devices")]
        public async Task<ActionResult<LoRaDeviceDto>> CreateDevice([FromBody] CreateLoRaDeviceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var device = await _loRaSystemService.AddDeviceAsync(dto);
                return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "新增設備時發生錯誤", error = ex.Message });
            }
        }

        [HttpPut("devices/{id}")]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] UpdateLoRaDeviceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _loRaSystemService.UpdateDeviceAsync(id, dto);
                if (!updated)
                {
                    return NotFound(new { message = $"找不到編號為 {id} 的設備" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "更新設備資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpDelete("devices/{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            try
            {
                var deleted = await _loRaSystemService.RemoveDeviceAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = $"找不到編號為 {id} 的設備" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "刪除設備資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpGet("readings/latest")]
        public async Task<ActionResult<IEnumerable<LoRaMeterReadingDto>>> GetLatestReadings([FromQuery] int? deviceId = null, [FromQuery] int count = 10)
        {
            try
            {
                if (count <= 0)
                {
                    count = 10;
                }

                var readings = await _loRaSystemService.GetLatestReadingsAsync(deviceId, count);
                return Ok(readings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入最新抄表資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpGet("devices/{id}/readings")]
        public async Task<ActionResult<IEnumerable<LoRaMeterReadingDto>>> GetDeviceReadings(int id, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            try
            {
                var readings = await _loRaSystemService.GetDeviceReadingsAsync(id, from, to);
                return Ok(readings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入抄表資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpPost("readings")]
        public async Task<ActionResult<LoRaMeterReadingDto>> AddReading([FromBody] CreateLoRaMeterReadingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var reading = await _loRaSystemService.AddMeterReadingAsync(dto);
                return CreatedAtAction(nameof(GetDeviceReadings), new { id = reading.DeviceId }, reading);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "新增抄表資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpGet("alerts")]
        public async Task<ActionResult<IEnumerable<LoRaAlertDto>>> GetAlerts([FromQuery] string? status = null, [FromQuery] int? deviceId = null)
        {
            try
            {
                var alerts = await _loRaSystemService.GetAlertsAsync(status, deviceId);
                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "載入警報資料時發生錯誤", error = ex.Message });
            }
        }

        [HttpPost("alerts")]
        public async Task<ActionResult<LoRaAlertDto>> CreateAlert([FromBody] CreateLoRaAlertDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var alert = await _loRaSystemService.CreateAlertAsync(dto);
                return CreatedAtAction(nameof(GetAlerts), new { status = alert.Status }, alert);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "新增警報時發生錯誤", error = ex.Message });
            }
        }

        [HttpPut("alerts/{id}/status")]
        public async Task<IActionResult> UpdateAlertStatus(int id, [FromBody] UpdateLoRaAlertStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _loRaSystemService.UpdateAlertStatusAsync(id, dto);
                if (!updated)
                {
                    return NotFound(new { message = $"找不到編號為 {id} 的警報" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "更新警報狀態時發生錯誤", error = ex.Message });
            }
        }
    }
}
