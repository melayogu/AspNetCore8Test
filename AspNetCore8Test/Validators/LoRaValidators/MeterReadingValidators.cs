using AspNetCore8Test.Models.DTOs.LoRaDtos;
using FluentValidation;

namespace AspNetCore8Test.Validators.LoRaValidators
{
    public class CreateMeterReadingDtoValidator : AbstractValidator<CreateMeterReadingDto>
    {
        public CreateMeterReadingDtoValidator()
        {
            RuleFor(x => x.Timestamp)
                .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
                .WithMessage("讀值時間不可晚於現在");

            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Unit)
                .NotEmpty()
                .MaximumLength(10);

            RuleFor(x => x.SignalStrength)
                .InclusiveBetween(-150, -20).WithMessage("訊號強度需介於 -150 到 -20 dBm");

            RuleFor(x => x.SignalToNoiseRatio)
                .InclusiveBetween(-30, 30).WithMessage("訊噪比需介於 -30 到 30 dB");

            RuleFor(x => x.BatteryLevel)
                .InclusiveBetween(0, 100);

            RuleFor(x => x.TransmissionStatus)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
