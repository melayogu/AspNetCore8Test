using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Services.LoRaServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers.LoRaControllers
{
    [ApiController]
    [Route("api/lora/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ILoRaDeviceService _deviceService;
        private readonly ILoRaGatewayService _gatewayService;
        private readonly IValidator<CreateLoRaDeviceDto> _createValidator;
        private readonly IValidator<UpdateLoRaDeviceDto> _updateValidator;
        private readonly IValidator<UpdateLoRaDeviceStatusDto> _statusValidator;

        public DevicesController(
            ILoRaDeviceService deviceService,
            ILoRaGatewayService gatewayService,
            IValidator<CreateLoRaDeviceDto> createValidator,
            IValidator<UpdateLoRaDeviceDto> updateValidator,
            IValidator<UpdateLoRaDeviceStatusDto> statusValidator)
        {
            _deviceService = deviceService;
            _gatewayService = gatewayService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _statusValidator = statusValidator;
        }

        /// <summary>
        /// 取得所有 LoRa 終端設備
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoRaDeviceDto>>> GetDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }

        /// <summary>
        /// 依照設備 ID 取得單一設備
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LoRaDeviceDto>> GetDeviceById(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound($"找不到 ID 為 {id} 的設備");
            }

            return Ok(device);
        }

        /// <summary>
        /// 依照 Device EUI 查詢設備
        /// </summary>
        [HttpGet("by-eui/{deviceEui}")]
        public async Task<ActionResult<LoRaDeviceDto>> GetDeviceByEui(string deviceEui)
        {
            var device = await _deviceService.GetDeviceByEuiAsync(deviceEui);
            if (device == null)
            {
                return NotFound($"找不到 Device EUI 為 {deviceEui} 的設備");
            }

            return Ok(device);
        }

        /// <summary>
        /// 依照閘道器查詢其下所有設備
        /// </summary>
        [HttpGet("gateway/{gatewayEui}")]
        public async Task<ActionResult<IEnumerable<LoRaDeviceDto>>> GetDevicesByGateway(string gatewayEui)
        {
            var devices = await _deviceService.GetDevicesByGatewayAsync(gatewayEui);
            return Ok(devices);
        }

        /// <summary>
        /// 依照狀態篩選設備
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<LoRaDeviceDto>>> GetDevicesByStatus(string status)
        {
            var devices = await _deviceService.GetDevicesByStatusAsync(status);
            return Ok(devices);
        }

        /// <summary>
        /// 搜尋設備
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<LoRaDeviceDto>>> SearchDevices([FromQuery] string searchTerm = "")
        {
            var devices = await _deviceService.SearchDevicesAsync(searchTerm);
            return Ok(devices);
        }

        /// <summary>
        /// 新增 LoRa 終端設備
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LoRaDeviceDto>> CreateDevice(CreateLoRaDeviceDto createDto)
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

            if (await _deviceService.DeviceEuiExistsAsync(createDto.DeviceEui))
            {
                return Conflict($"Device EUI {createDto.DeviceEui} 已存在");
            }

            if (!await _gatewayService.GatewayEuiExistsAsync(createDto.GatewayEui))
            {
                return BadRequest($"找不到對應的 Gateway EUI {createDto.GatewayEui}");
            }

            var device = await _deviceService.CreateDeviceAsync(createDto);
            return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
        }

        /// <summary>
        /// 更新設備資訊
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<LoRaDeviceDto>> UpdateDevice(int id, UpdateLoRaDeviceDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("路徑參數與資料中的 Id 不一致");
            }

            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            if (!await _gatewayService.GatewayEuiExistsAsync(updateDto.GatewayEui))
            {
                return BadRequest($"找不到對應的 Gateway EUI {updateDto.GatewayEui}");
            }

            var device = await _deviceService.UpdateDeviceAsync(id, updateDto);
            if (device == null)
            {
                return NotFound($"找不到 ID 為 {id} 的設備");
            }

            return Ok(device);
        }

        /// <summary>
        /// 更新設備狀態、電量等即時資訊
        /// </summary>
        [HttpPatch("{deviceEui}/status")]
        public async Task<ActionResult<LoRaDeviceDto>> UpdateDeviceStatus(string deviceEui, UpdateLoRaDeviceStatusDto statusDto)
        {
            var validationResult = await _statusValidator.ValidateAsync(statusDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(statusDto.GatewayEui) &&
                !await _gatewayService.GatewayEuiExistsAsync(statusDto.GatewayEui))
            {
                return BadRequest($"找不到對應的 Gateway EUI {statusDto.GatewayEui}");
            }

            var device = await _deviceService.UpdateDeviceStatusAsync(deviceEui, statusDto);
            if (device == null)
            {
                return NotFound($"找不到 Device EUI 為 {deviceEui} 的設備");
            }

            return Ok(device);
        }

        /// <summary>
        /// 刪除設備
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var success = await _deviceService.DeleteDeviceAsync(id);
            if (!success)
            {
                return NotFound($"找不到 ID 為 {id} 的設備");
            }

            return NoContent();
        }
    }
}
