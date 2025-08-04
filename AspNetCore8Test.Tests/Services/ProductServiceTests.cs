using AspNetCore8Test.Models;
using AspNetCore8Test.Services;
using Xunit;

namespace AspNetCore8Test.Tests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnOnlyActiveProducts()
        {
            // Arrange
            var service = new ProductService();

            // Act
            var products = await service.GetAllProductsAsync();

            // Assert
            Assert.NotNull(products);
            Assert.True(products.All(p => p.IsActive));
            Assert.Equal(3, products.Count()); // 預設有3個產品
        }

        [Fact]
        public async Task GetProductByIdAsync_WithValidId_ShouldReturnProduct()
        {
            // Arrange
            var service = new ProductService();

            // Act
            var product = await service.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
            Assert.Equal("iPhone 15", product.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var service = new ProductService();

            // Act
            var product = await service.GetProductByIdAsync(999);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldCreateProductWithCorrectProperties()
        {
            // Arrange
            var service = new ProductService();
            var newProduct = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 199.99m,
                Category = "電子產品"
            };

            // Act
            var createdProduct = await service.CreateProductAsync(newProduct);

            // Assert
            Assert.NotNull(createdProduct);
            Assert.True(createdProduct.Id > 0);
            Assert.Equal("Test Product", createdProduct.Name);
            Assert.Equal("Test Description", createdProduct.Description);
            Assert.Equal(199.99m, createdProduct.Price);
            Assert.Equal("電子產品", createdProduct.Category);
            Assert.True(createdProduct.IsActive);
            Assert.True(createdProduct.CreatedDate <= DateTime.Now);
        }

        [Fact]
        public async Task UpdateProductAsync_WithValidProduct_ShouldUpdateSuccessfully()
        {
            // Arrange
            var service = new ProductService();
            var existingProduct = await service.GetProductByIdAsync(1);
            Assert.NotNull(existingProduct);

            var updateProduct = new Product
            {
                Id = existingProduct.Id,
                Name = "Updated iPhone 15",
                Description = "Updated Description",
                Price = 30000m,
                Category = "電子產品",
                IsActive = true
            };

            // Act
            var updatedProduct = await service.UpdateProductAsync(updateProduct);

            // Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated iPhone 15", updatedProduct.Name);
            Assert.Equal("Updated Description", updatedProduct.Description);
            Assert.Equal(30000m, updatedProduct.Price);
            Assert.Equal(existingProduct.CreatedDate, updatedProduct.CreatedDate); // 創建日期不變
        }

        [Fact]
        public async Task UpdateProductAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var service = new ProductService();
            var updateProduct = new Product
            {
                Id = 999,
                Name = "Non-existent Product",
                Description = "Description",
                Price = 100m,
                Category = "電子產品"
            };

            // Act
            var result = await service.UpdateProductAsync(updateProduct);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProductAsync_WithValidId_ShouldMarkAsInactive()
        {
            // Arrange
            var service = new ProductService();
            var productId = 1;

            // 確認產品存在且為活躍狀態
            var productBefore = await service.GetProductByIdAsync(productId);
            Assert.NotNull(productBefore);
            Assert.True(productBefore.IsActive);

            // Act
            var deleteResult = await service.DeleteProductAsync(productId);

            // Assert
            Assert.True(deleteResult);

            // 確認產品已被軟刪除（不會在 GetProductByIdAsync 中返回）
            var productAfter = await service.GetProductByIdAsync(productId);
            Assert.Null(productAfter);
        }

        [Fact]
        public async Task DeleteProductAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var service = new ProductService();

            // Act
            var result = await service.DeleteProductAsync(999);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1, true)]  // 存在的產品
        [InlineData(2, true)]  // 存在的產品
        [InlineData(3, true)]  // 存在的產品
        [InlineData(999, false)]  // 不存在的產品
        public async Task ProductExistsAsync_ShouldReturnCorrectResult(int id, bool expectedExists)
        {
            // Arrange
            var service = new ProductService();

            // Act
            var exists = await service.ProductExistsAsync(id);

            // Assert
            Assert.Equal(expectedExists, exists);
        }

        [Fact]
        public async Task ProductExistsAsync_AfterSoftDelete_ShouldReturnFalse()
        {
            // Arrange
            var service = new ProductService();
            var productId = 1;

            // 確認產品存在
            var existsBefore = await service.ProductExistsAsync(productId);
            Assert.True(existsBefore);

            // 軟刪除產品
            await service.DeleteProductAsync(productId);

            // Act
            var existsAfter = await service.ProductExistsAsync(productId);

            // Assert
            Assert.False(existsAfter);
        }
    }
}
