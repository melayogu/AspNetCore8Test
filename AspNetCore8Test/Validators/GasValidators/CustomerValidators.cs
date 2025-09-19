using AspNetCore8Test.Models.DTOs.GasDTOs;
using FluentValidation;

namespace AspNetCore8Test.Validators.GasValidators
{
    public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerDtoValidator()
        {
            RuleFor(x => x.CustomerNumber)
                .NotEmpty().WithMessage("客戶編號是必填的")
                .MaximumLength(100).WithMessage("客戶編號最多100個字元")
                .Matches(@"^[A-Z]\d{3,6}$").WithMessage("客戶編號格式應為一個大寫字母後跟3-6位數字，例如：C001");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("客戶姓名是必填的")
                .MaximumLength(200).WithMessage("客戶姓名最多200個字元")
                .MinimumLength(2).WithMessage("客戶姓名至少2個字元");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("客戶類型是必填的")
                .Must(BeValidCustomerType).WithMessage("客戶類型必須是：住宅、商業、工業 其中之一");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("地址是必填的")
                .MaximumLength(500).WithMessage("地址最多500個字元")
                .MinimumLength(10).WithMessage("地址至少10個字元");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("電話號碼是必填的")
                .Matches(@"^(\d{2,4}-\d{6,8}|\d{10})$")
                .WithMessage("電話號碼格式不正確，請使用 02-12345678 或 0912345678 格式");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("請輸入有效的電子郵件地址")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.TaxId)
                .Matches(@"^\d{8}$").WithMessage("統一編號必須是8位數字")
                .When(x => !string.IsNullOrEmpty(x.TaxId));
        }

        private bool BeValidCustomerType(string type)
        {
            var validTypes = new[] { "住宅", "商業", "工業" };
            return validTypes.Contains(type);
        }
    }

    public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerDtoValidator()
        {
            Include(new CreateCustomerDtoValidator());

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("客戶ID必須大於0");

            RuleFor(x => x.Status)
                .Must(BeValidStatus).WithMessage("狀態必須是：Active、Suspended、Terminated 其中之一");
        }

        private bool BeValidStatus(string status)
        {
            var validStatuses = new[] { "Active", "Suspended", "Terminated" };
            return validStatuses.Contains(status);
        }
    }
}