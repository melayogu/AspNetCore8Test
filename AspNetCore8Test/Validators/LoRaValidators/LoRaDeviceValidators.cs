using AspNetCore8Test.Models.DTOs.LoRaDtos;
using FluentValidation;

namespace AspNetCore8Test.Validators.LoRaValidators
{
    public class CreateLoRaDeviceDtoValidator : AbstractValidator<CreateLoRaDeviceDto>
    {
        public CreateLoRaDeviceDtoValidator()
        {
            RuleFor(x => x.DeviceEui)
                .NotEmpty().WithMessage("DeviceEUI 為必填")
                .Matches("^[A-Fa-f0-9]{16}$").WithMessage("DeviceEUI 需為 16 位元十六進位字串");

            RuleFor(x => x.MeterNumber)
                .NotEmpty().WithMessage("表號為必填")
                .MaximumLength(50);

            RuleFor(x => x.MeterType)
                .NotEmpty().WithMessage("請指定表計類型")
                .MaximumLength(50);

            RuleFor(x => x.FirmwareVersion)
                .NotEmpty().WithMessage("請填寫韌體版本")
                .MaximumLength(20);

            RuleFor(x => x.GatewayId)
                .GreaterThan(0).WithMessage("請指定有效的閘道器");

            RuleFor(x => x.InstallationLocation)
                .NotEmpty().WithMessage("請填寫裝機位置")
                .MaximumLength(120);

            RuleFor(x => x.InstallationNotes)
                .MaximumLength(200);

            RuleFor(x => x.InstalledAt)
                .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
                .WithMessage("裝機日期不可晚於現在");
        }
    }

    public class UpdateLoRaDeviceDtoValidator : AbstractValidator<UpdateLoRaDeviceDto>
    {
        public UpdateLoRaDeviceDtoValidator()
        {
            Include(new CreateLoRaDeviceDtoValidator());

            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Status)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.BatteryLevel)
                .InclusiveBetween(0, 100).WithMessage("電池電量必須介於 0 到 100 之間");

            RuleFor(x => x.SignalStrength)
                .InclusiveBetween(-150, -20).WithMessage("訊號強度需介於 -150 到 -20 dBm");

            RuleFor(x => x.SignalToNoiseRatio)
                .InclusiveBetween(-30, 30).WithMessage("訊噪比需介於 -30 到 30 dB");

            RuleFor(x => x.LastReadingValue)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.LastReadingUnit)
                .MaximumLength(10);

            RuleFor(x => x.LastCommunicationAt)
                .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
                .WithMessage("最後通訊時間不可晚於現在");
        }
    }
}
