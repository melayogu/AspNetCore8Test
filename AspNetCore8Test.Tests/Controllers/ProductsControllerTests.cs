using AspNetCore8Test.Controllers;
using AspNetCore8Test.Models;
using AspNetCore8Test.Models.DTOs;
using AspNetCore8Test.Services;
using AspNetCore8Test.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AspNetCore8Test.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _createValidator = new CreateProductDtoValidator();
            _updateValidator = new UpdateProductDtoValidator();
            _controller = new ProductsController(_mockProductService.Object, _createValidator, _updateValidator);
        }

        [Theory]
        [InlineData(2, "Product 1", "Product 2")]
        [InlineData(3, "電子產品", "服飾", "家具")]
        [InlineData(1, "單一商品")]
        public async Task GetProducts_ShouldReturnOkWithProducts(int productCount, params string[] productNames)
        {
            // Arrange
            var products = new List<Product>();
            for (int i = 0; i < productCount; i++)
            {
                products.Add(new Product 
                { 
                    Id = i + 1, 
                    Name = productNames.Length > i ? productNames[i] : $"Product {i + 1}", 
                    Description = $"Desc {i + 1}", 
                    Price = (i + 1) * 100, 
                    Category = "電子產品", 
                    CreatedDate = DateTime.Now, 
                    IsActive = true 
                });
            }
            _mockProductService.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductResponseDto>>(okResult.Value);
            Assert.Equal(productCount, returnedProducts.Count());
        }

        [Theory]
        [InlineData(1, "Test Product", "Test Desc", 100, "電子產品")]
        [InlineData(5, "手機", "智慧型手機", 15000, "通訊設備")]
        [InlineData(10, "筆記型電腦", "高效能筆電", 45000, "電腦")]
        public async Task GetProduct_WithValidId_ShouldReturnOkWithProduct(int id, string name, string description, decimal price, string category)
        {
            // Arrange
            var product = new Product { Id = id, Name = name, Description = description, Price = price, Category = category, CreatedDate = DateTime.Now, IsActive = true };
            _mockProductService.Setup(s => s.GetProductByIdAsync(id)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductResponseDto>(okResult.Value);
            Assert.Equal(id, returnedProduct.Id);
            Assert.Equal(name, returnedProduct.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetProduct_WithInvalidId_ShouldReturnBadRequest(int id)
        {
            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("商品 ID 必須大於 0", badRequestResult.Value);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        [InlineData(5555)]
        public async Task GetProduct_WithNonExistentId_ShouldReturnNotFound(int id)
        {
            // Arrange
            _mockProductService.Setup(s => s.GetProductByIdAsync(id)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"找不到 ID 為 {id} 的商品", notFoundResult.Value);
        }

        [Theory]
        [InlineData(4, "New Product", "New Description", 299.99, "電子產品")]
        [InlineData(5, "智慧手錶", "多功能智慧手錶", 8999.99, "電子產品")]
        [InlineData(6, "無線耳機", "高音質無線耳機", 3999.99, "電子產品")]
        public async Task CreateProduct_WithValidData_ShouldReturnCreatedAtAction(int expectedId, string name, string description, decimal price, string category)
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = name,
                Description = description,
                Price = price,
                Category = category
            };

            var createdProduct = new Product
            {
                Id = expectedId,
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                Category = createDto.Category,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _mockProductService.Setup(s => s.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.CreateProduct(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductResponseDto>(createdAtActionResult.Value);
            Assert.Equal(expectedId, returnedProduct.Id);
            Assert.Equal(name, returnedProduct.Name);
            Assert.Equal("GetProduct", createdAtActionResult.ActionName);
        }

        [Theory]
        [InlineData("", "Description", 100, "電子產品")] // 無效的名稱 - 空字串
        [InlineData("A", "Description", 100, "電子產品")] // 無效的名稱 - 太短
        [InlineData("Valid Product", "", 100, "電子產品")] // 無效的描述 - 空字串
        [InlineData("Valid Product", "Valid Description", 0, "電子產品")] // 無效的價格
        [InlineData("Valid Product", "Valid Description", -100, "電子產品")] // 無效的價格 - 負數
        public async Task CreateProduct_WithInvalidData_ShouldReturnBadRequest(string name, string description, decimal price, string category)
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = name,
                Description = description,
                Price = price,
                Category = category
            };

            // Act
            var result = await _controller.CreateProduct(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("Product@123")]
        public async Task CreateProduct_WithInvalidName_ShouldReturnBadRequest(string invalidName)
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = invalidName,
                Description = "Valid Description",
                Price = 100,
                Category = "電子產品"
            };

            // Act
            var result = await _controller.CreateProduct(createDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Theory]
        [InlineData(1, "Updated Product", "Updated Description", 199.99, "電子產品", true)]
        [InlineData(2, "修改的商品", "更新的商品描述", 2999.99, "家居", false)]
        [InlineData(3, "New Name", "New Detail", 599.50, "書籍", true)]
        public async Task UpdateProduct_WithValidData_ShouldReturnOkWithUpdatedProduct(int id, string name, string description, decimal price, string category, bool isActive)
        {
            // Arrange
            var updateDto = new UpdateProductDto
            {
                Id = id,
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                IsActive = isActive
            };

            var existingProduct = new Product
            {
                Id = id,
                Name = "Original Product",
                Description = "Original Description",
                Price = 100,
                Category = "電子產品",
                CreatedDate = DateTime.Now.AddDays(-1),
                IsActive = true
            };

            var updatedProduct = new Product
            {
                Id = id,
                Name = updateDto.Name,
                Description = updateDto.Description,
                Price = updateDto.Price,
                Category = updateDto.Category,
                CreatedDate = existingProduct.CreatedDate,
                IsActive = updateDto.IsActive
            };

            _mockProductService.Setup(s => s.GetProductByIdAsync(id)).ReturnsAsync(existingProduct);
            _mockProductService.Setup(s => s.UpdateProductAsync(It.IsAny<Product>())).ReturnsAsync(updatedProduct);

            // Act
            var result = await _controller.UpdateProduct(id, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductResponseDto>(okResult.Value);
            Assert.Equal(name, returnedProduct.Name);
            Assert.Equal(description, returnedProduct.Description);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(5, 10)]
        [InlineData(100, 200)]
        public async Task UpdateProduct_WithMismatchedId_ShouldReturnBadRequest(int urlId, int dtoId)
        {
            // Arrange
            var updateDto = new UpdateProductDto { Id = dtoId };

            // Act
            var result = await _controller.UpdateProduct(urlId, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("路徑中的 ID 與請求體中的 ID 不匹配", badRequestResult.Value);
        }

        [Theory]
        [InlineData(999, "Updated Product", "Updated Description", 199.99, "電子產品", true)]
        [InlineData(1000, "不存在的商品", "商品描述", 500.00, "書籍", false)]
        [InlineData(5555, "Test Product", "Test Description", 1000.00, "運動", true)]
        public async Task UpdateProduct_WithNonExistentProduct_ShouldReturnNotFound(int id, string name, string description, decimal price, string category, bool isActive)
        {
            // Arrange
            var updateDto = new UpdateProductDto
            {
                Id = id,
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                IsActive = isActive
            };

            _mockProductService.Setup(s => s.GetProductByIdAsync(id)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.UpdateProduct(id, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"找不到 ID 為 {id} 的商品", notFoundResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public async Task DeleteProduct_WithValidId_ShouldReturnNoContent(int id)
        {
            // Arrange
            _mockProductService.Setup(s => s.ProductExistsAsync(id)).ReturnsAsync(true);
            _mockProductService.Setup(s => s.DeleteProductAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteProduct_WithInvalidId_ShouldReturnBadRequest(int id)
        {
            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("商品 ID 必須大於 0", badRequestResult.Value);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        [InlineData(8888)]
        public async Task DeleteProduct_WithNonExistentId_ShouldReturnNotFound(int id)
        {
            // Arrange
            _mockProductService.Setup(s => s.ProductExistsAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"找不到 ID 為 {id} 的商品", notFoundResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task DeleteProduct_WhenDeleteFails_ShouldReturnBadRequest(int id)
        {
            // Arrange
            _mockProductService.Setup(s => s.ProductExistsAsync(id)).ReturnsAsync(true);
            _mockProductService.Setup(s => s.DeleteProductAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("刪除商品失敗", badRequestResult.Value);
        }
    }
}
