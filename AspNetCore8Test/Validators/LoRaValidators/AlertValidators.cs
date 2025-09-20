using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using FluentValidation;

namespace AspNetCore8Test.Validators.LoRaValidators
{
    public class CreateLoRaAlertDtoValidator : AbstractValidator<CreateLoRaAlertDto>
    {
        private static readonly string[] AllowedSeverities = { "Low", "Medium", "High", "Critical" };

        public CreateLoRaAlertDtoValidator()
        {
            RuleFor(x => x.DeviceEui)
                .NotEmpty()
                .Matches("^[A-Fa-f0-9]{16}$");

            RuleFor(x => x.GatewayEui)
                .Matches("^[A-Fa-f0-9]{16}$")
                .When(x => !string.IsNullOrWhiteSpace(x.GatewayEui));

            RuleFor(x => x.AlertType)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Severity)
                .Must(severity => AllowedSeverities.Contains(severity))
                .WithMessage($"告警等級必須為 {string.Join("、", AllowedSeverities)} 之一");

            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.SuggestedAction)
                .MaximumLength(500);

            RuleFor(x => x.OccurredAt)
                .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
                .WithMessage("告警時間不可晚於現在");
        }
    }

    public class AcknowledgeLoRaAlertDtoValidator : AbstractValidator<AcknowledgeLoRaAlertDto>
    {
        public AcknowledgeLoRaAlertDtoValidator()
        {
            RuleFor(x => x.AcknowledgedAt)
                .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
                .When(x => x.AcknowledgedAt.HasValue)
                .WithMessage("確認時間不可晚於現在");

            RuleFor(x => x.SuggestedAction)
                .MaximumLength(500);
        }
    }
}
