using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Services.LoRaServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Test.Controllers.LoRaControllers
{
    [ApiController]
    [Route("api/lora/[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly ILoRaAlertService _alertService;
        private readonly ILoRaDeviceService _deviceService;
        private readonly ILoRaGatewayService _gatewayService;
        private readonly IValidator<CreateLoRaAlertDto> _createValidator;
        private readonly IValidator<AcknowledgeLoRaAlertDto> _ackValidator;

        public AlertsController(
            ILoRaAlertService alertService,
            ILoRaDeviceService deviceService,
            ILoRaGatewayService gatewayService,
            IValidator<CreateLoRaAlertDto> createValidator,
            IValidator<AcknowledgeLoRaAlertDto> ackValidator)
        {
            _alertService = alertService;
            _deviceService = deviceService;
            _gatewayService = gatewayService;
            _createValidator = createValidator;
            _ackValidator = ackValidator;
        }

        /// <summary>
        /// 取得告警清單
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoRaAlertDto>>> GetAlerts([FromQuery] bool onlyActive = false)
        {
            var alerts = onlyActive
                ? await _alertService.GetActiveAlertsAsync()
                : await _alertService.GetAllAlertsAsync();

            return Ok(alerts);
        }

        /// <summary>
        /// 取得指定告警詳情
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LoRaAlertDto>> GetAlert(int id)
        {
            var alert = await _alertService.GetAlertByIdAsync(id);
            if (alert == null)
            {
                return NotFound($"找不到 ID 為 {id} 的告警");
            }

            return Ok(alert);
        }

        /// <summary>
        /// 取得指定設備的告警
        /// </summary>
        [HttpGet("device/{deviceEui}")]
        public async Task<ActionResult<IEnumerable<LoRaAlertDto>>> GetAlertsByDevice(string deviceEui)
        {
            if (!await _deviceService.DeviceEuiExistsAsync(deviceEui))
            {
                return NotFound($"找不到 Device EUI 為 {deviceEui} 的設備");
            }

            var alerts = await _alertService.GetAlertsByDeviceAsync(deviceEui);
            return Ok(alerts);
        }

        /// <summary>
        /// 新增告警
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LoRaAlertDto>> CreateAlert(CreateLoRaAlertDto createDto)
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

            if (!string.IsNullOrWhiteSpace(createDto.GatewayEui) &&
                !await _gatewayService.GatewayEuiExistsAsync(createDto.GatewayEui))
            {
                return BadRequest($"找不到 Gateway EUI 為 {createDto.GatewayEui} 的閘道器");
            }

            var alert = await _alertService.CreateAlertAsync(createDto);
            return CreatedAtAction(nameof(GetAlert), new { id = alert.Id }, alert);
        }

        /// <summary>
        /// 確認或解除告警
        /// </summary>
        [HttpPut("{id:int}/acknowledge")]
        public async Task<ActionResult<LoRaAlertDto>> AcknowledgeAlert(int id, AcknowledgeLoRaAlertDto acknowledgeDto)
        {
            var validationResult = await _ackValidator.ValidateAsync(acknowledgeDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            var alert = await _alertService.AcknowledgeAlertAsync(id, acknowledgeDto);
            if (alert == null)
            {
                return NotFound($"找不到 ID 為 {id} 的告警");
            }

            return Ok(alert);
        }
    }
}
