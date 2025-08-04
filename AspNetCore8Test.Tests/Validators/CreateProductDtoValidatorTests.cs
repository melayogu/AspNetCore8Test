using AspNetCore8Test.Models.DTOs;
using AspNetCore8Test.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AspNetCore8Test.Tests.Validators
{
    public class CreateProductDtoValidatorTests
    {
        private readonly CreateProductDtoValidator _validator;

        public CreateProductDtoValidatorTests()
        {
            _validator = new CreateProductDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new CreateProductDto 
            { 
                Name = "",
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("商品名稱不能為空");
        }

        [Theory]
        [InlineData("A")] // 太短
        public void Should_Have_Error_When_Name_Length_Is_Invalid(string name)
        {
            var model = new CreateProductDto 
            { 
                Name = name,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("商品名稱長度必須在 2 到 100 個字元之間");
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Too_Long()
        {
            var longName = new string('A', 101); // 101 個字元
            var model = new CreateProductDto 
            { 
                Name = longName,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("商品名稱長度必須在 2 到 100 個字元之間");
        }

        [Fact]
        public void Should_Have_Error_When_Name_With_Chinese_Characters_Is_Too_Long()
        {
            // 101 個中文字元
            var longChineseName = new string('中', 101);
            var model = new CreateProductDto 
            { 
                Name = longChineseName,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("商品名稱長度必須在 2 到 100 個字元之間");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Name_With_Chinese_Characters_Is_Valid_Length()
        {
            // 100 個中文字元（邊界值測試）
            var validChineseName = new string('商', 100);
            var model = new CreateProductDto 
            { 
                Name = validChineseName,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("Test@Product")] // 包含特殊字符
        [InlineData("Product#123")] // 包含特殊字符
        [InlineData("商品$名稱")] // 包含特殊字符
        public void Should_Have_Error_When_Name_Contains_Invalid_Characters(string name)
        {
            var model = new CreateProductDto 
            { 
                Name = name,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("商品名稱只能包含字母、數字、中文和空格");
        }

        [Theory]
        [InlineData("iPhone 15")] // 英文數字空格
        [InlineData("蘋果手機")] // 中文
        [InlineData("Product123")] // 英文數字
        [InlineData("商品 123")] // 中文數字空格
        public void Should_Not_Have_Error_When_Name_Is_Valid(string name)
        {
            var model = new CreateProductDto 
            { 
                Name = name,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = "",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("商品描述不能為空");
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Too_Long()
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = new string('A', 501), // 501 個字元
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("商品描述不能超過 500 個字元");
        }

        [Fact]
        public void Should_Have_Error_When_Description_With_Chinese_Characters_Is_Too_Long()
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = new string('描', 501), // 501 個中文字元
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("商品描述不能超過 500 個字元");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Description_With_Chinese_Characters_Is_Valid_Length()
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = new string('述', 500), // 500 個中文字元（邊界值）
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Mixed_Language_Name_Is_Too_Long()
        {
            // 混合中英文測試：50個中文 + 51個英文 = 101個字元
            var mixedName = new string('中', 50) + new string('A', 51);
            var model = new CreateProductDto 
            { 
                Name = mixedName,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("商品名稱長度必須在 2 到 100 個字元之間");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Mixed_Language_Name_Is_Valid_Length()
        {
            // 混合中英文測試：50個中文 + 50個英文 = 100個字元
            var mixedName = new string('商', 50) + new string('A', 50);
            var model = new CreateProductDto 
            { 
                Name = mixedName,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData(0)] // 等於0
        [InlineData(-1)] // 小於0
        [InlineData(-100)] // 負數
        public void Should_Have_Error_When_Price_Is_Zero_Or_Negative(decimal price)
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = "Valid description",
                Price = price,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage("商品價格必須大於 0");
        }

        [Fact]
        public void Should_Have_Error_When_Price_Is_Too_High()
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = "Valid description",
                Price = 1000001, // 超過上限
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage("商品價格不能超過 1,000,000");
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(100)]
        [InlineData(999999)]
        [InlineData(1000000)]
        public void Should_Not_Have_Error_When_Price_Is_Valid(decimal price)
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = "Valid description",
                Price = price,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Should_Have_Error_When_Category_Is_Empty()
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = "Valid description",
                Price = 100,
                Category = ""
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Category)
                  .WithErrorMessage("商品分類不能為空");
        }

        [Theory]
        [InlineData("無效分類")]
        [InlineData("Electronics")]
        [InlineData("其他")]
        public void Should_Have_Error_When_Category_Is_Invalid(string category)
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = "Valid description",
                Price = 100,
                Category = category
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Category)
                  .WithErrorMessage("商品分類必須是：電子產品、服飾、書籍、運動、家居 其中之一");
        }

        [Theory]
        [InlineData("電子產品")]
        [InlineData("服飾")]
        [InlineData("書籍")]
        [InlineData("運動")]
        [InlineData("家居")]
        public void Should_Not_Have_Error_When_Category_Is_Valid(string category)
        {
            var model = new CreateProductDto 
            { 
                Name = "Valid Product",
                Description = "Valid description",
                Price = 100,
                Category = category
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            var model = new CreateProductDto
            {
                Name = "iPhone 15 Pro",
                Description = "最新款的 iPhone，具有強大的性能和出色的攝影功能",
                Price = 35900,
                Category = "電子產品"
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
