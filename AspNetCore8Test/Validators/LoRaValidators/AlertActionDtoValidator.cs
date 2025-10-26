using AspNetCore8Test.Models.DTOs.LoRaDtos;
using FluentValidation;

namespace AspNetCore8Test.Validators.LoRaValidators
{
    public class AlertActionDtoValidator : AbstractValidator<AlertActionDto>
    {
        public AlertActionDtoValidator()
        {
            RuleFor(x => x.OperatorName)
                .NotEmpty().WithMessage("請提供操作人員名稱")
                .MaximumLength(50);
        }
    }
}
