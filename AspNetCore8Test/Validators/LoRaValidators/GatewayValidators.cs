using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using FluentValidation;

namespace AspNetCore8Test.Validators.LoRaValidators
{
    public class CreateLoRaGatewayDtoValidator : AbstractValidator<CreateLoRaGatewayDto>
    {
        private static readonly string[] AllowedBackhaulTypes = { "Fiber", "Ethernet", "Cellular", "Satellite" };

        public CreateLoRaGatewayDtoValidator()
        {
            RuleFor(x => x.GatewayEui)
                .NotEmpty()
                .Matches("^[A-Fa-f0-9]{16}$").WithMessage("Gateway EUI 必須為 16 碼十六進位字串");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Location)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.IpAddress)
                .NotEmpty()
                .Matches("^((25[0-5]|2[0-4]\\d|[01]?\\d\\d?)\\.){3}(25[0-5]|2[0-4]\\d|[01]?\\d\\d?)$")
                .WithMessage("請輸入正確的 IPv4 位址");

            RuleFor(x => x.FirmwareVersion)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(x => x.PacketSuccessRate)
                .InclusiveBetween(0, 100);

            RuleFor(x => x.BackhaulType)
                .Must(type => AllowedBackhaulTypes.Contains(type))
                .WithMessage($"回程連線類型必須為 {string.Join("、", AllowedBackhaulTypes)} 之一");

            RuleFor(x => x.Notes)
                .MaximumLength(500);
        }
    }

    public class UpdateLoRaGatewayDtoValidator : AbstractValidator<UpdateLoRaGatewayDto>
    {
        private static readonly string[] AllowedStatuses = { "Normal", "Maintenance", "Offline", "Alarm" };

        public UpdateLoRaGatewayDtoValidator()
        {
            Include(new CreateLoRaGatewayDtoValidator());

            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Status)
                .Must(status => AllowedStatuses.Contains(status))
                .WithMessage($"狀態必須為 {string.Join("、", AllowedStatuses)} 之一");

            RuleFor(x => x.ConnectedDevices)
                .GreaterThanOrEqualTo(0);
        }
    }

    public class UpdateLoRaGatewayStatusDtoValidator : AbstractValidator<UpdateLoRaGatewayStatusDto>
    {
        private static readonly string[] AllowedStatuses = { "Normal", "Maintenance", "Offline", "Alarm" };

        public UpdateLoRaGatewayStatusDtoValidator()
        {
            RuleFor(x => x.PacketSuccessRate)
                .InclusiveBetween(0, 100)
                .When(x => x.PacketSuccessRate.HasValue);

            RuleFor(x => x.ConnectedDevices)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ConnectedDevices.HasValue);

            RuleFor(x => x.Status)
                .Must(status => string.IsNullOrWhiteSpace(status) || AllowedStatuses.Contains(status!))
                .WithMessage($"狀態必須為 {string.Join("、", AllowedStatuses)} 之一");
        }
    }
}
