using AspNetCore8Test.Models;
using AspNetCore8Test.Services;

namespace AspNetCore8Test.Tests.Examples
{
    /// <summary>
    /// 測試夾具類別 - 用於共用資源初始化
    /// </summary>
    public class ProductServiceFixture : IDisposable
    {
        private bool _disposed = false;
        
        public IProductService ProductService { get; private set; }
        public List<Product> InitialProducts { get; private set; }

        public ProductServiceFixture()
        {
            // 測試前置動作（非測資），搭配 constructor
            Console.WriteLine("=== ProductServiceFixture 初始化開始 ===");
            
            // 初始化服務
            ProductService = new ProductService();
            
            // 建立初始測試資料
            InitialProducts = new List<Product>();
            
            // 添加一些測試資料
            var testProducts = new[]
            {
                new Product { Name = "測試產品 1", Description = "描述 1", Price = 100, Category = "測試分類 A" },
                new Product { Name = "測試產品 2", Description = "描述 2", Price = 200, Category = "測試分類 B" },
                new Product { Name = "測試產品 3", Description = "描述 3", Price = 300, Category = "測試分類 A" },
                new Product { Name = "測試產品 4", Description = "描述 4", Price = 400, Category = "測試分類 C" },
                new Product { Name = "測試產品 5", Description = "描述 5", Price = 500, Category = "測試分類 B" }
            };

            foreach (var product in testProducts)
            {
                var createdProduct = ProductService.CreateProductAsync(product).Result;
                InitialProducts.Add(createdProduct);
            }

            Console.WriteLine($"已初始化 {InitialProducts.Count} 個測試產品");
            Console.WriteLine("=== ProductServiceFixture 初始化完成 ===");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // 清理資源
                Console.WriteLine("=== ProductServiceFixture 清理開始 ===");
                
                // 在實際專案中，這裡可能會清理資料庫連線、檔案等資源
                InitialProducts?.Clear();
                
                Console.WriteLine("=== ProductServiceFixture 清理完成 ===");
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// IClassFixture&lt;T&gt; 測試範例 - 測試初始化（非測資）
    /// 適用情境：共用資料庫連線、初始化邏輯
    /// 特性說明：測試前置動作（非測資），搭配 constructor
    /// </summary>
    public class IClassFixtureExampleTests : IClassFixture<ProductServiceFixture>
    {
        private readonly ProductServiceFixture _fixture;

        public IClassFixtureExampleTests(ProductServiceFixture fixture)
        {
            _fixture = fixture;
            Console.WriteLine($"測試類別建構子執行，當前有 {_fixture.InitialProducts.Count} 個初始產品");
        }

        [Fact]
        public async Task GetAllProductsAsync_WithFixtureData_ShouldIncludeInitialProducts()
        {
            // Arrange - 使用夾具中的服務
            var service = _fixture.ProductService;

            // Act
            var allProducts = await service.GetAllProductsAsync();
            var productsList = allProducts.ToList();

            // Assert
            Assert.NotEmpty(productsList);
            
            // 應該包含夾具中建立的測試產品 + 服務預設的產品
            Assert.True(productsList.Count >= _fixture.InitialProducts.Count);
            
            // 檢查是否包含夾具建立的產品
            foreach (var initialProduct in _fixture.InitialProducts)
            {
                Assert.Contains(productsList, p => p.Name == initialProduct.Name);
            }
        }

        [Fact]
        public async Task GetProductByIdAsync_WithFixtureProduct_ShouldReturnCorrectProduct()
        {
            // Arrange
            var service = _fixture.ProductService;
            var firstInitialProduct = _fixture.InitialProducts[0];

            // Act
            var foundProduct = await service.GetProductByIdAsync(firstInitialProduct.Id);

            // Assert
            Assert.NotNull(foundProduct);
            Assert.Equal(firstInitialProduct.Name, foundProduct.Name);
            Assert.Equal(firstInitialProduct.Price, foundProduct.Price);
            Assert.Equal(firstInitialProduct.Category, foundProduct.Category);
        }

        [Fact]
        public async Task CreateProductAsync_WithFixtureService_ShouldAddToExistingProducts()
        {
            // Arrange
            var service = _fixture.ProductService;
            var initialCount = (await service.GetAllProductsAsync()).Count();
            
            var newProduct = new Product
            {
                Name = "新增測試產品",
                Description = "透過夾具服務新增",
                Price = 999,
                Category = "新增分類"
            };

            // Act
            var createdProduct = await service.CreateProductAsync(newProduct);
            var finalCount = (await service.GetAllProductsAsync()).Count();

            // Assert
            Assert.NotNull(createdProduct);
            Assert.True(createdProduct.Id > 0);
            Assert.Equal(initialCount + 1, finalCount);
        }

        [Theory]
        [InlineData("測試分類 A")]
        [InlineData("測試分類 B")]
        [InlineData("測試分類 C")]
        public async Task GetProductsByCategory_WithFixtureData_ShouldReturnCorrectCount(string category)
        {
            // Arrange
            var service = _fixture.ProductService;
            var expectedCount = _fixture.InitialProducts.Count(p => p.Category == category);

            // Act
            var allProducts = await service.GetAllProductsAsync();
            var categoryProducts = allProducts.Where(p => p.Category == category).ToList();

            // Assert
            Assert.Equal(expectedCount, categoryProducts.Count);
            Assert.All(categoryProducts, product => Assert.Equal(category, product.Category));
        }

        [Fact]
        public async Task UpdateProductAsync_WithFixtureProduct_ShouldMaintainProductCount()
        {
            // Arrange
            var service = _fixture.ProductService;
            var productToUpdate = _fixture.InitialProducts[0];
            var initialCount = (await service.GetAllProductsAsync()).Count();

            var updatedProduct = new Product
            {
                Id = productToUpdate.Id,
                Name = productToUpdate.Name + " (已更新)",
                Description = productToUpdate.Description + " (已更新)",
                Price = productToUpdate.Price + 100,
                Category = productToUpdate.Category,
                CreatedDate = productToUpdate.CreatedDate,
                IsActive = productToUpdate.IsActive
            };

            // Act
            var result = await service.UpdateProductAsync(updatedProduct);
            var finalCount = (await service.GetAllProductsAsync()).Count();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(initialCount, finalCount); // 更新不應該改變產品總數
            Assert.Contains("(已更新)", result.Name);
            // 第一個產品的價格是 100，加上 100 等於 200
            Assert.Equal(200, result.Price);
        }

        [Fact]
        public async Task DeleteProductAsync_WithFixtureProduct_ShouldReduceProductCount()
        {
            // Arrange
            var service = _fixture.ProductService;
            var productToDelete = _fixture.InitialProducts[_fixture.InitialProducts.Count - 1];
            var initialCount = (await service.GetAllProductsAsync()).Count();

            // Act
            var deleteResult = await service.DeleteProductAsync(productToDelete.Id);
            var finalCount = (await service.GetAllProductsAsync()).Count();

            // Assert
            Assert.True(deleteResult);
            Assert.Equal(initialCount - 1, finalCount);
            
            // 確認產品已被刪除
            var deletedProduct = await service.GetProductByIdAsync(productToDelete.Id);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task ProductExistsAsync_WithFixtureProducts_ShouldReturnCorrectResults()
        {
            // Arrange
            var service = _fixture.ProductService;

            // Act & Assert
            foreach (var product in _fixture.InitialProducts)
            {
                var exists = await service.ProductExistsAsync(product.Id);
                Assert.True(exists, $"產品 {product.Name} (ID: {product.Id}) 應該存在");
            }

            // 測試不存在的產品
            var nonExistentExists = await service.ProductExistsAsync(99999);
            Assert.False(nonExistentExists, "不存在的產品ID應該返回false");
        }
    }
}
