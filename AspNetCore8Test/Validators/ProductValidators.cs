using FluentValidation;
using AspNetCore8Test.Models.DTOs;

namespace AspNetCore8Test.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("商品名稱不能為空")
                .Length(2, 100).WithMessage("商品名稱長度必須在 2 到 100 個字元之間")
                .Matches(@"^[a-zA-Z0-9\u4e00-\u9fa5\s]+$").WithMessage("商品名稱只能包含字母、數字、中文和空格");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("商品描述不能為空")
                .MaximumLength(500).WithMessage("商品描述不能超過 500 個字元");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("商品價格必須大於 0")
                .LessThanOrEqualTo(1000000).WithMessage("商品價格不能超過 1,000,000");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("商品分類不能為空")
                .Must(BeValidCategory).WithMessage("商品分類必須是：電子產品、服飾、書籍、運動、家居 其中之一");
        }

        private static bool BeValidCategory(string category)
        {
            var validCategories = new[] { "電子產品", "服飾", "書籍", "運動", "家居" };
            return validCategories.Contains(category);
        }
    }

    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("商品 ID 必須大於 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("商品名稱不能為空")
                .Length(2, 100).WithMessage("商品名稱長度必須在 2 到 100 個字元之間")
                .Matches(@"^[a-zA-Z0-9\u4e00-\u9fa5\s]+$").WithMessage("商品名稱只能包含字母、數字、中文和空格");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("商品描述不能為空")
                .MaximumLength(500).WithMessage("商品描述不能超過 500 個字元");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("商品價格必須大於 0")
                .LessThanOrEqualTo(1000000).WithMessage("商品價格不能超過 1,000,000");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("商品分類不能為空")
                .Must(BeValidCategory).WithMessage("商品分類必須是：電子產品、服飾、書籍、運動、家居 其中之一");
        }

        private static bool BeValidCategory(string category)
        {
            var validCategories = new[] { "電子產品", "服飾", "書籍", "運動", "家居" };
            return validCategories.Contains(category);
        }
    }
}
