using AspNetCore8Test.Models;
using AspNetCore8Test.Services;
using System.Collections;

namespace AspNetCore8Test.Tests.Examples
{
    /// <summary>
    /// [Theory] + ClassData 測試範例 - 複雜邏輯生成的測資
    /// 適用情境：測資來源需要封裝邏輯時
    /// 特性說明：建立類別繼承 IEnumerable&lt;object[]&gt;
    /// </summary>
    public class TheoryClassDataTests
    {
        [Theory]
        [ClassData(typeof(ProductValidationTestData))]
        public async Task ProductService_Validation_ShouldMatchExpectedBehavior(
            Product product, string operationType, bool expectedSuccess, string expectedMessage)
        {
            // Arrange
            var service = new ProductService();

            // Act & Assert
            switch (operationType.ToLower())
            {
                case "create":
                    if (expectedSuccess)
                    {
                        var created = await service.CreateProductAsync(product);
                        Assert.NotNull(created);
                        Assert.True(created.Id > 0);
                    }
                    else
                    {
                        // 檢查產品資料的有效性
                        bool isValid = IsProductValid(product);
                        Assert.False(isValid, expectedMessage);
                    }
                    break;

                case "update":
                    // 先創建一個產品以便更新
                    var existingProduct = await service.CreateProductAsync(
                        new Product { Name = "原始產品", Price = 100, Category = "測試" });
                    
                    product.Id = existingProduct.Id;
                    
                    if (expectedSuccess)
                    {
                        var updated = await service.UpdateProductAsync(product);
                        Assert.NotNull(updated);
                    }
                    else
                    {
                        bool isValid = IsProductValid(product);
                        Assert.False(isValid, expectedMessage);
                    }
                    break;

                case "delete":
                    if (expectedSuccess)
                    {
                        // 先創建產品再刪除
                        var toDelete = await service.CreateProductAsync(product);
                        var deleted = await service.DeleteProductAsync(toDelete.Id);
                        Assert.True(deleted);
                    }
                    else
                    {
                        // 刪除不存在的產品
                        var deleted = await service.DeleteProductAsync(-1);
                        Assert.False(deleted);
                    }
                    break;
            }
        }

        [Theory]
        [ClassData(typeof(ProductPriceRangeTestData))]
        public void Product_PriceRange_ShouldBeValid(decimal price, string priceCategory, bool isValid)
        {
            // Arrange
            var product = new Product
            {
                Name = $"產品 - {priceCategory}",
                Price = price,
                Category = "測試"
            };

            // Act
            bool actualValid = product.Price > 0 && product.Price <= 999999;

            // Assert
            Assert.Equal(isValid, actualValid);
        }

        [Theory]
        [ClassData(typeof(ProductCategoryTestData))]
        public async Task ProductService_FilterByCategory_ShouldReturnCorrectProducts(
            List<Product> productsToAdd, string filterCategory, int expectedAdditionalCount)
        {
            // Arrange
            var service = new ProductService();
            
            // 先獲取現有該分類的產品數量
            var initialProducts = await service.GetAllProductsAsync();
            var initialCount = initialProducts.Count(p => p.Category == filterCategory);
            
            // 添加測試產品
            foreach (var product in productsToAdd)
            {
                await service.CreateProductAsync(product);
            }

            // Act
            var allProducts = await service.GetAllProductsAsync();
            var filteredProducts = allProducts.Where(p => p.Category == filterCategory);

            // Assert
            Assert.Equal(initialCount + expectedAdditionalCount, filteredProducts.Count());
        }

        private static bool IsProductValid(Product product)
        {
            return !string.IsNullOrWhiteSpace(product.Name) && 
                   product.Price > 0 &&
                   !string.IsNullOrWhiteSpace(product.Category);
        }
    }

    /// <summary>
    /// 產品驗證測試資料類別
    /// </summary>
    public class ProductValidationTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // 有效的創建測試案例
            yield return new object[]
            {
                new Product { Name = "有效產品", Price = 100, Category = "電子" },
                "create",
                true,
                "有效的產品應該能夠創建"
            };

            // 無效的創建測試案例 - 空名稱
            yield return new object[]
            {
                new Product { Name = "", Price = 100, Category = "電子" },
                "create",
                false,
                "產品名稱不能為空"
            };

            // 無效的創建測試案例 - 負價格
            yield return new object[]
            {
                new Product { Name = "負價格產品", Price = -50, Category = "電子" },
                "create",
                false,
                "產品價格不能為負數"
            };

            // 無效的創建測試案例 - 空分類
            yield return new object[]
            {
                new Product { Name = "無分類產品", Price = 100, Category = "" },
                "create",
                false,
                "產品分類不能為空"
            };

            // 有效的更新測試案例
            yield return new object[]
            {
                new Product { Name = "更新後產品", Price = 200, Category = "更新分類" },
                "update",
                true,
                "有效的產品應該能夠更新"
            };

            // 無效的更新測試案例
            yield return new object[]
            {
                new Product { Name = "", Price = 200, Category = "分類" },
                "update",
                false,
                "更新時產品名稱不能為空"
            };

            // 刪除測試案例
            yield return new object[]
            {
                new Product { Name = "待刪除產品", Price = 100, Category = "測試" },
                "delete",
                true,
                "有效的產品應該能夠刪除"
            };

            yield return new object[]
            {
                new Product { Name = "不存在產品", Price = 100, Category = "測試" },
                "delete",
                false,
                "不存在的產品無法刪除"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// 產品價格範圍測試資料類別
    /// </summary>
    public class ProductPriceRangeTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // 低價位產品
            for (decimal price = 1; price <= 100; price += 25)
            {
                yield return new object[] { price, "低價位", true };
            }

            // 中價位產品
            for (decimal price = 101; price <= 1000; price += 200)
            {
                yield return new object[] { price, "中價位", true };
            }

            // 高價位產品
            for (decimal price = 1001; price <= 10000; price += 2000)
            {
                yield return new object[] { price, "高價位", true };
            }

            // 超高價位產品
            for (decimal price = 10001; price <= 50000; price += 10000)
            {
                yield return new object[] { price, "超高價位", true };
            }

            // 無效價格
            yield return new object[] { 0, "零價格", false };
            yield return new object[] { -1, "負價格", false };
            yield return new object[] { -100, "大負數", false };
            yield return new object[] { 1000000, "超過限制", false };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// 產品分類測試資料類別
    /// </summary>
    public class ProductCategoryTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // 電子產品測試案例
            yield return new object[]
            {
                new List<Product>
                {
                    new() { Name = "iPhone", Price = 30000, Category = "電子產品" },
                    new() { Name = "iPad", Price = 20000, Category = "電子產品" },
                    new() { Name = "Nike鞋", Price = 3000, Category = "運動" }
                },
                "電子產品",
                2
            };

            // 運動用品測試案例
            yield return new object[]
            {
                new List<Product>
                {
                    new() { Name = "籃球", Price = 500, Category = "運動" },
                    new() { Name = "跑鞋", Price = 2000, Category = "運動" },
                    new() { Name = "書籍", Price = 300, Category = "教育" },
                    new() { Name = "羽球拍", Price = 1500, Category = "運動" }
                },
                "運動",
                3
            };

            // 書籍測試案例
            yield return new object[]
            {
                new List<Product>
                {
                    new() { Name = "程式設計", Price = 500, Category = "書籍" },
                    new() { Name = "歷史書", Price = 300, Category = "書籍" },
                    new() { Name = "手機", Price = 15000, Category = "電子產品" }
                },
                "書籍",
                2
            };

            // 沒有該分類的測試案例
            yield return new object[]
            {
                new List<Product>
                {
                    new() { Name = "產品A", Price = 100, Category = "分類A" },
                    new() { Name = "產品B", Price = 200, Category = "分類B" }
                },
                "不存在的分類",
                0
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
