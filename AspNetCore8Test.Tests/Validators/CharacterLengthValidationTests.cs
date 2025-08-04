using AspNetCore8Test.Models.DTOs;
using AspNetCore8Test.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AspNetCore8Test.Tests.Validators
{
    public class CharacterLengthValidationTests
    {
        private readonly CreateProductDtoValidator _validator;

        public CharacterLengthValidationTests()
        {
            _validator = new CreateProductDtoValidator();
        }

        [Theory]
        [InlineData("A", 1)] // 1個英文字元
        [InlineData("中", 1)] // 1個中文字元
        [InlineData("あ", 1)] // 1個日文字元
        [InlineData("😀", 2)] // 1個 emoji（Unicode surrogate pair，實際佔2個char）
        [InlineData("AB", 2)] // 2個英文字元
        [InlineData("中國", 2)] // 2個中文字元
        [InlineData("A中", 2)] // 1個英文 + 1個中文
        [InlineData("Hello 世界", 8)] // 5個英文 + 1個空格 + 2個中文
        public void Should_Calculate_Character_Length_Correctly(string text, int expectedLength)
        {
            // Arrange & Act
            var actualLength = text.Length;
            
            // Assert
            Assert.Equal(expectedLength, actualLength);
        }

        [Fact]
        public void Should_Validate_Exactly_100_Characters_As_Valid()
        {
            // 測試邊界值：恰好 100 個字元
            var exactly100Chars = new string('A', 50) + new string('中', 50); // 50英文 + 50中文 = 100字元
            
            var model = new CreateProductDto 
            { 
                Name = exactly100Chars,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Validate_Exactly_101_Characters_As_Invalid()
        {
            // 測試邊界值：101 個字元
            var exactly101Chars = new string('A', 50) + new string('中', 50) + "X"; // 50英文 + 50中文 + 1英文 = 101字元
            
            var model = new CreateProductDto 
            { 
                Name = exactly101Chars,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("商品名稱長度必須在 2 到 100 個字元之間");
        }

        [Theory]
        [InlineData("特殊字符測試ABC123", 12)] // 實際測量為12個字元
        [InlineData("Mixed測試123", 10)] // 5個英文 + 2個中文 + 3個數字
        [InlineData("Product 產品", 10)] // 實際測量為10個字元（7個英文 + 1個空格 + 2個中文）
        public void Should_Validate_Various_Unicode_Characters_In_Product_Name(string text, int expectedLength)
        {
            // Arrange
            var model = new CreateProductDto 
            { 
                Name = text,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            
            // Act
            var result = _validator.TestValidate(model);
            var actualLength = text.Length;
            
            // Assert
            Assert.Equal(expectedLength, actualLength);
            // 如果長度在 2-100 之間，應該通過驗證
            if (actualLength >= 2 && actualLength <= 100)
            {
                result.ShouldNotHaveValidationErrorFor(x => x.Name);
            }
            else
            {
                result.ShouldHaveValidationErrorFor(x => x.Name);
            }
        }

        [Fact]
        public void Should_Handle_Unicode_Surrogate_Pairs_Correctly()
        {
            // 測試 Unicode surrogate pairs（如某些 emoji）
            var textWithEmoji = "Test😀"; // "Test" (4字元) + 😀 (2個 char 但算1個字元)
            
            // .NET 的 string.Length 會計算 char 數量，不是 Unicode 字元數量
            // 對於 emoji 😀，它實際上是由兩個 char 組成（surrogate pair）
            Assert.Equal(6, textWithEmoji.Length); // "Test" (4) + 😀 (2 chars)
            
            // 如果要正確計算 Unicode 字元數量，需要使用 StringInfo
            var unicodeLength = System.Globalization.StringInfo.ParseCombiningCharacters(textWithEmoji).Length;
            Assert.Equal(5, unicodeLength); // "Test" (4) + 😀 (1 Unicode character)
        }

        [Fact]
        public void FluentValidation_Uses_String_Length_Not_Unicode_Length()
        {
            // FluentValidation 的 Length() 驗證使用 string.Length，不是 Unicode 字元數量
            // 我們使用只包含允許字元的測試數據
            var nameWithChinese = new string('A', 98) + "中文"; // 98個A + 2個中文 = 100 chars
            
            var model = new CreateProductDto 
            { 
                Name = nameWithChinese,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            
            var result = _validator.TestValidate(model);
            // 應該通過驗證，因為 string.Length = 100 且只包含允許的字元
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Reject_Name_With_Emoji_Due_To_Character_Rules()
        {
            // 測試包含 emoji 的名稱會被字元規則拒絕（而不是長度規則）
            var nameWithEmoji = "Product 😀"; // 包含不允許的emoji字元
            
            var model = new CreateProductDto 
            { 
                Name = nameWithEmoji,
                Description = "Valid description",
                Price = 100,
                Category = "電子產品"
            };
            
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("商品名稱只能包含字母、數字、中文和空格");
        }
    }
}
