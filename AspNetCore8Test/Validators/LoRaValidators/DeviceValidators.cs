using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using FluentValidation;

namespace AspNetCore8Test.Validators.LoRaValidators
{
    public class CreateLoRaDeviceDtoValidator : AbstractValidator<CreateLoRaDeviceDto>
    {
        private static readonly string[] AllowedMeterTypes = { "Water", "Gas", "Electricity" };
        private static readonly string[] AllowedFrequencyPlans = { "AS923", "EU868", "US915", "AU915" };

        public CreateLoRaDeviceDtoValidator()
        {
            RuleFor(x => x.DeviceEui)
                .NotEmpty().WithMessage("Device EUI 為必填欄位")
                .Matches("^[A-Fa-f0-9]{16}$").WithMessage("Device EUI 必須為 16 碼十六進位字串");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("設備名稱為必填欄位")
                .MaximumLength(200);

            RuleFor(x => x.MeterType)
                .Must(type => AllowedMeterTypes.Contains(type))
                .WithMessage($"儀表種類必須為 {string.Join("、", AllowedMeterTypes)} 之一");

            RuleFor(x => x.FirmwareVersion)
                .NotEmpty().WithMessage("韌體版本為必填欄位")
                .MaximumLength(50);

            RuleFor(x => x.BatteryLevel)
                .InclusiveBetween(0, 100).WithMessage("電量需介於 0-100 之間");

            RuleFor(x => x.InstallLocation)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.GatewayEui)
                .NotEmpty()
                .Matches("^[A-Fa-f0-9]{16}$").WithMessage("閘道器 EUI 必須為 16 碼十六進位字串");

            RuleFor(x => x.ApplicationKey)
                .NotEmpty()
                .Matches("^[A-Fa-f0-9]{32}$").WithMessage("Application Key 必須為 32 碼十六進位字串");

            RuleFor(x => x.FrequencyPlan)
                .Must(plan => AllowedFrequencyPlans.Contains(plan))
                .WithMessage($"頻段計畫必須為 {string.Join("、", AllowedFrequencyPlans)} 之一");

            RuleFor(x => x.AlertThreshold)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.InstallDate)
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                .WithMessage("安裝日期不可晚於今日");

            RuleFor(x => x.Notes)
                .MaximumLength(500);
        }
    }

    public class UpdateLoRaDeviceDtoValidator : AbstractValidator<UpdateLoRaDeviceDto>
    {
        private static readonly string[] AllowedStatuses = { "Active", "Maintenance", "Suspended", "Retired" };

        public UpdateLoRaDeviceDtoValidator()
        {
            Include(new CreateLoRaDeviceDtoValidator());

            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Status)
                .Must(status => AllowedStatuses.Contains(status))
                .WithMessage($"狀態必須為 {string.Join("、", AllowedStatuses)} 之一");
        }
    }

    public class UpdateLoRaDeviceStatusDtoValidator : AbstractValidator<UpdateLoRaDeviceStatusDto>
    {
        private static readonly string[] AllowedStatuses = { "Active", "Maintenance", "Suspended", "Retired", "Error" };

        public UpdateLoRaDeviceStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(status => AllowedStatuses.Contains(status))
                .WithMessage($"狀態必須為 {string.Join("、", AllowedStatuses)} 之一");

            RuleFor(x => x.BatteryLevel)
                .InclusiveBetween(0, 100)
                .When(x => x.BatteryLevel.HasValue)
                .WithMessage("電量需介於 0-100 之間");

            RuleFor(x => x.GatewayEui)
                .Matches("^[A-Fa-f0-9]{16}$")
                .When(x => !string.IsNullOrWhiteSpace(x.GatewayEui))
                .WithMessage("閘道器 EUI 必須為 16 碼十六進位字串");
        }
    }
}
