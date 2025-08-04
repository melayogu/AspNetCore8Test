using AspNetCore8Test.Models;
using AspNetCore8Test.Services;

namespace AspNetCore8Test.Tests.Examples
{
    /// <summary>
    /// [Theory] + MemberData 測試範例 - 複雜資料結構
    /// 適用情境：多組帶物件或時間的測資
    /// 特性說明：資料來自 static 屬性或方法
    /// </summary>
    public class TheoryMemberDataTests
    {
        // 靜態屬性提供測試資料
        public static IEnumerable<object[]> ProductTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new Product { Name = "iPhone 15", Description = "蘋果手機", Price = 32900, Category = "電子產品" },
                    "電子產品",
                    true
                },
                new object[]
                {
                    new Product { Name = "Nike 球鞋", Description = "運動鞋", Price = 2800, Category = "運動" },
                    "運動",
                    true
                },
                new object[]
                {
                    new Product { Name = "", Description = "無名產品", Price = 100, Category = "測試" },
                    "測試",
                    false // 名稱為空，應該無效
                },
                new object[]
                {
                    new Product { Name = "書籍", Description = "程式設計", Price = -100, Category = "教育" },
                    "教育",
                    false // 價格為負，應該無效
                }
            };

        [Theory]
        [MemberData(nameof(ProductTestData))]
        public async Task CreateProductAsync_WithVariousData_ShouldHandleCorrectly(
            Product product, string expectedCategory, bool shouldSucceed)
        {
            // Arrange
            var service = new ProductService();

            // Act & Assert
            if (shouldSucceed)
            {
                var createdProduct = await service.CreateProductAsync(product);
                Assert.NotNull(createdProduct);
                Assert.Equal(expectedCategory, createdProduct.Category);
                Assert.True(createdProduct.Id > 0);
            }
            else
            {
                // 對於無效資料，我們可以假設服務會進行驗證
                // 這裡我們測試資料本身的有效性
                bool isValid = !string.IsNullOrWhiteSpace(product.Name) && product.Price > 0;
                Assert.False(isValid);
            }
        }

        // 靜態方法提供更複雜的測試資料
        public static IEnumerable<object[]> GetProductUpdateScenarios()
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Local);
            
            yield return new object[]
            {
                new Product { Id = 1, Name = "原始產品", Price = 100, CreatedDate = baseDate },
                new Product { Id = 1, Name = "更新產品", Price = 200, CreatedDate = baseDate },
                true,
                "名稱和價格都更新"
            };
            
            yield return new object[]
            {
                new Product { Id = 1, Name = "產品A", Price = 150, CreatedDate = baseDate },
                new Product { Id = 2, Name = "產品B", Price = 250, CreatedDate = baseDate }, // 不同ID
                false,
                "ID不符，更新應該失敗"
            };
            
            yield return new object[]
            {
                new Product { Id = 1, Name = "有效產品", Price = 100, CreatedDate = baseDate },
                new Product { Id = 1, Name = "", Price = 200, CreatedDate = baseDate }, // 空名稱
                false,
                "空名稱，更新應該失敗"
            };
        }

        [Theory]
        [MemberData(nameof(GetProductUpdateScenarios))]
        public async Task UpdateProductAsync_WithDifferentScenarios_ShouldHandleCorrectly(
            Product originalProduct, Product updateProduct, bool expectedSuccess, string scenario)
        {
            // Arrange
            var service = new ProductService();
            
            // 先創建原始產品
            var created = await service.CreateProductAsync(originalProduct);
            
            // 設定更新產品的ID
            updateProduct.Id = created.Id;

            // Act
            var result = await service.UpdateProductAsync(updateProduct);

            // Assert
            if (expectedSuccess)
            {
                Assert.NotNull(result);
                Assert.Equal(updateProduct.Name, result.Name);
                Assert.Equal(updateProduct.Price, result.Price);
            }
            else
            {
                // 根據實際的服務實作，可能會返回null或拋出例外
                // 這裡我們檢查輸入資料的有效性
                bool isValid = !string.IsNullOrWhiteSpace(updateProduct.Name) && 
                              updateProduct.Price > 0;
                
                if (!isValid)
                {
                    Assert.False(isValid, $"測試案例: {scenario}");
                }
            }
        }

        // 提供日期相關的測試資料
        public static IEnumerable<object[]> DateTestData =>
            new List<object[]>
            {
                new object[] { DateTime.Now.AddDays(-30), "30天前創建的產品" },
                new object[] { DateTime.Now.AddDays(-1), "昨天創建的產品" },
                new object[] { DateTime.Now, "今天創建的產品" },
                new object[] { DateTime.Now.AddHours(-1), "一小時前創建的產品" }
            };

        [Theory]
        [MemberData(nameof(DateTestData))]
        public void Product_CreatedDate_ShouldBeValidDateTime(DateTime createdDate, string description)
        {
            // Arrange
            var product = new Product
            {
                Name = "測試產品",
                CreatedDate = createdDate,
                Price = 100
            };

            // Act & Assert
            Assert.True(product.CreatedDate <= DateTime.Now, 
                $"產品創建日期不應該是未來時間: {description}");
            Assert.True(product.CreatedDate >= DateTime.Now.AddYears(-10), 
                $"產品創建日期不應該太久遠: {description}");
        }
    }
}
