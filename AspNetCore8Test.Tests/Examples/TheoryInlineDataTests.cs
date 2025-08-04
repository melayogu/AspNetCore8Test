using AspNetCore8Test.Models;

namespace AspNetCore8Test.Tests.Examples
{
    /// <summary>
    /// [Theory] + InlineData 測試範例 - 多組參數
    /// 適用情境：簡單多組測資（值類型為主）
    /// 特性說明：直接在屬性列舉每組參數
    /// </summary>
    public class TheoryInlineDataTests
    {
        [Theory]
        [InlineData("iPhone", "手機", 1000, true)]
        [InlineData("MacBook", "筆電", 50000, true)]
        [InlineData("", "無名稱產品", 100, false)] // 空名稱應該無效
        [InlineData("正常產品", "", 500, true)] // 空描述可以接受
        [InlineData("負價格產品", "測試", -100, false)] // 負價格應該無效
        public void Product_Validation_ShouldMatchExpectedResult(
            string name, string description, decimal price, bool expectedValid)
        {
            // Arrange
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Category = "測試"
            };

            // Act
            bool isValid = IsProductValid(product);

            // Assert
            Assert.Equal(expectedValid, isValid);
        }

        [Theory]
        [InlineData(0, false)]      // ID 0 無效
        [InlineData(-1, false)]     // 負數 ID 無效
        [InlineData(1, true)]       // 正數 ID 有效
        [InlineData(999999, true)]  // 大數字 ID 有效
        public void ProductId_Validation_ShouldMatchExpectedResult(int id, bool expectedValid)
        {
            // Arrange & Act
            bool isValid = id > 0;

            // Assert
            Assert.Equal(expectedValid, isValid);
        }

        [Theory]
        [InlineData("電子產品", true)]
        [InlineData("服飾", true)]
        [InlineData("書籍", true)]
        [InlineData("", false)]        // 空字串無效
        [InlineData("   ", false)]     // 只有空白無效
        public void Category_Validation_ShouldMatchExpectedResult(string category, bool expectedValid)
        {
            // Act
            bool isValid = !string.IsNullOrWhiteSpace(category);

            // Assert
            Assert.Equal(expectedValid, isValid);
        }

        [Theory]
        [InlineData(0.01, true)]      // 最小有效價格
        [InlineData(1, true)]         // 一般價格
        [InlineData(99999.99, true)]  // 高價格
        [InlineData(0, false)]        // 零價格無效
        [InlineData(-0.01, false)]    // 負價格無效
        public void Price_Validation_ShouldMatchExpectedResult(decimal price, bool expectedValid)
        {
            // Act
            bool isValid = price > 0;

            // Assert
            Assert.Equal(expectedValid, isValid);
        }

        private static bool IsProductValid(Product product)
        {
            return !string.IsNullOrWhiteSpace(product.Name) && 
                   product.Price > 0;
        }
    }
}
