using AspNetCore8Test.Models.DTOs;
using AspNetCore8Test.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AspNetCore8Test.Tests.Validators
{
    public class UpdateProductDtoValidatorTests
    {
        private readonly UpdateProductDtoValidator _validator;

        public UpdateProductDtoValidatorTests()
        {
            _validator = new UpdateProductDtoValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Have_Error_When_Id_Is_Zero_Or_Negative(int id)
        {
            var model = new UpdateProductDto 
            { 
                Id = id,
                Name = "Valid Product",
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("商品 ID 必須大於 0");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999)]
        public void Should_Not_Have_Error_When_Id_Is_Valid(int id)
        {
            var model = new UpdateProductDto 
            { 
                Id = id,
                Name = "Valid Product",
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new UpdateProductDto 
            { 
                Id = 1,
                Name = "",
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("商品名稱不能為空");
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var model = new UpdateProductDto 
            { 
                Id = 1,
                Name = "Valid Product",
                Description = "",
                Price = 100,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("商品描述不能為空");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Have_Error_When_Price_Is_Zero_Or_Negative(decimal price)
        {
            var model = new UpdateProductDto 
            { 
                Id = 1,
                Name = "Valid Product",
                Description = "Valid description",
                Price = price,
                Category = "電子產品"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage("商品價格必須大於 0");
        }

        [Theory]
        [InlineData("無效分類")]
        [InlineData("")]
        public void Should_Have_Error_When_Category_Is_Invalid(string category)
        {
            var model = new UpdateProductDto 
            { 
                Id = 1,
                Name = "Valid Product",
                Description = "Valid description",
                Price = 100,
                Category = category
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            var model = new UpdateProductDto
            {
                Id = 1,
                Name = "Updated iPhone 15 Pro",
                Description = "更新後的 iPhone 描述",
                Price = 33900,
                Category = "電子產品",
                IsActive = true
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Accept_Any_IsActive_Value(bool isActive)
        {
            var model = new UpdateProductDto
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test description",
                Price = 100,
                Category = "電子產品",
                IsActive = isActive
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.IsActive);
        }
    }
}
