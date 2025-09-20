using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Services.LoRaServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers.LoRaControllers
{
    [ApiController]
    [Route("api/lora/[controller]")]
    public class GatewaysController : ControllerBase
    {
        private readonly ILoRaGatewayService _gatewayService;
        private readonly IValidator<CreateLoRaGatewayDto> _createValidator;
        private readonly IValidator<UpdateLoRaGatewayDto> _updateValidator;
        private readonly IValidator<UpdateLoRaGatewayStatusDto> _statusValidator;

        public GatewaysController(
            ILoRaGatewayService gatewayService,
            IValidator<CreateLoRaGatewayDto> createValidator,
            IValidator<UpdateLoRaGatewayDto> updateValidator,
            IValidator<UpdateLoRaGatewayStatusDto> statusValidator)
        {
            _gatewayService = gatewayService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _statusValidator = statusValidator;
        }

        /// <summary>
        /// 取得所有 LoRa 閘道器資訊
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoRaGatewayDto>>> GetGateways()
        {
            var gateways = await _gatewayService.GetAllGatewaysAsync();
            return Ok(gateways);
        }

        /// <summary>
        /// 依照閘道器 ID 取得單筆資料
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LoRaGatewayDto>> GetGatewayById(int id)
        {
            var gateway = await _gatewayService.GetGatewayByIdAsync(id);
            if (gateway == null)
            {
                return NotFound($"找不到 ID 為 {id} 的閘道器");
            }

            return Ok(gateway);
        }

        /// <summary>
        /// 依照 Gateway EUI 取得資料
        /// </summary>
        [HttpGet("by-eui/{gatewayEui}")]
        public async Task<ActionResult<LoRaGatewayDto>> GetGatewayByEui(string gatewayEui)
        {
            var gateway = await _gatewayService.GetGatewayByEuiAsync(gatewayEui);
            if (gateway == null)
            {
                return NotFound($"找不到 Gateway EUI 為 {gatewayEui} 的閘道器");
            }

            return Ok(gateway);
        }

        /// <summary>
        /// 依關鍵字搜尋閘道器
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<LoRaGatewayDto>>> SearchGateways([FromQuery] string searchTerm = "")
        {
            var gateways = await _gatewayService.SearchGatewaysAsync(searchTerm);
            return Ok(gateways);
        }

        /// <summary>
        /// 新增 LoRa 閘道器
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LoRaGatewayDto>> CreateGateway(CreateLoRaGatewayDto createDto)
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

            if (await _gatewayService.GatewayEuiExistsAsync(createDto.GatewayEui))
            {
                return Conflict($"Gateway EUI {createDto.GatewayEui} 已存在");
            }

            var gateway = await _gatewayService.CreateGatewayAsync(createDto);
            return CreatedAtAction(nameof(GetGatewayById), new { id = gateway.Id }, gateway);
        }

        /// <summary>
        /// 更新閘道器設定
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<LoRaGatewayDto>> UpdateGateway(int id, UpdateLoRaGatewayDto updateDto)
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

            var gateway = await _gatewayService.UpdateGatewayAsync(id, updateDto);
            if (gateway == null)
            {
                return NotFound($"找不到 ID 為 {id} 的閘道器");
            }

            return Ok(gateway);
        }

        /// <summary>
        /// 更新閘道器即時狀態，例如封包成功率與連線狀態
        /// </summary>
        [HttpPatch("{gatewayEui}/telemetry")]
        public async Task<ActionResult<LoRaGatewayDto>> UpdateGatewayTelemetry(string gatewayEui, UpdateLoRaGatewayStatusDto statusDto)
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

            var gateway = await _gatewayService.UpdateGatewayStatusAsync(gatewayEui, statusDto);
            if (gateway == null)
            {
                return NotFound($"找不到 Gateway EUI 為 {gatewayEui} 的閘道器");
            }

            return Ok(gateway);
        }

        /// <summary>
        /// 刪除閘道器
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGateway(int id)
        {
            var success = await _gatewayService.DeleteGatewayAsync(id);
            if (!success)
            {
                return NotFound($"找不到 ID 為 {id} 的閘道器");
            }

            return NoContent();
        }
    }
}
