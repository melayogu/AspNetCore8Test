using AspNetCore8Test.Models;
using AspNetCore8Test.Services;

namespace AspNetCore8Test.Tests.Examples
{
    /// <summary>
    /// [Fact] 測試範例 - 單一測試方法
    /// 適用情境：固定測資、不需參數
    /// 特性說明：最基本、最常見的寫法
    /// </summary>
    public class FactExampleTests
    {
        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllActiveProducts()
        {
            // Arrange
            var service = new ProductService();

            // Act
            var products = await service.GetAllProductsAsync();

            // Assert
            Assert.NotNull(products);
            Assert.True(products.Count() >= 3); // 預設有 3 筆資料
            Assert.All(products, product => Assert.True(product.IsActive));
        }

        [Fact]
        public async Task CreateProductAsync_ShouldAssignIdAndSetDefaults()
        {
            // Arrange
            var service = new ProductService();
            var newProduct = new Product
            {
                Name = "測試產品",
                Description = "這是一個測試產品",
                Price = 100,
                Category = "測試"
            };

            // Act
            var createdProduct = await service.CreateProductAsync(newProduct);

            // Assert
            Assert.True(createdProduct.Id > 0);
            Assert.True(createdProduct.IsActive);
            Assert.True(createdProduct.CreatedDate <= DateTime.Now);
            Assert.Equal("測試產品", createdProduct.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var service = new ProductService();
            var invalidId = 999;

            // Act
            var product = await service.GetProductByIdAsync(invalidId);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task ProductExistsAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var service = new ProductService();

            // Act
            var exists = await service.ProductExistsAsync(1); // 預設第一筆資料的 ID

            // Assert
            Assert.True(exists);
        }
    }
}
