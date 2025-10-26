using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using FluentValidation;

namespace AspNetCore8Test.Validators.LoRaValidators
{
    public class CreateMeterReadingDtoValidator : AbstractValidator<CreateMeterReadingDto>
    {
        private static readonly string[] AllowedQualityLevels = { "Excellent", "Good", "Fair", "Poor" };

        public CreateMeterReadingDtoValidator()
        {
            RuleFor(x => x.DeviceEui)
                .NotEmpty()
                .Matches("^[A-Fa-f0-9]{16}$");

            RuleFor(x => x.Timestamp)
                .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
                .WithMessage("讀值時間不可晚於現在");

            RuleFor(x => x.ReadingValue)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Consumption)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.FlowRate)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.BatteryLevel)
                .InclusiveBetween(0, 100);

            RuleFor(x => x.Snr)
                .GreaterThanOrEqualTo(-20)
                .LessThanOrEqualTo(20);

            RuleFor(x => x.Quality)
                .Must(q => AllowedQualityLevels.Contains(q))
                .WithMessage($"資料品質必須為 {string.Join("、", AllowedQualityLevels)} 之一");

            RuleFor(x => x.PayloadHex)
                .Must(BeValidHexPayload)
                .WithMessage("Payload 必須為偶數長度的十六進位字串");
        }

        private bool BeValidHexPayload(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                return true;
            }

            return payload.Length % 2 == 0 && payload.All(c => Uri.IsHexDigit(c));
        }
    }
}
