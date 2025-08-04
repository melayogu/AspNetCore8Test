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

        [Fact]
        public async Task GetProducts_ShouldReturnOkWithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Description = "Desc 1", Price = 100, Category = "電子產品", CreatedDate = DateTime.Now, IsActive = true },
                new Product { Id = 2, Name = "Product 2", Description = "Desc 2", Price = 200, Category = "服飾", CreatedDate = DateTime.Now, IsActive = true }
            };
            _mockProductService.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public async Task GetProduct_WithValidId_ShouldReturnOkWithProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", Description = "Test Desc", Price = 100, Category = "電子產品", CreatedDate = DateTime.Now, IsActive = true };
            _mockProductService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductResponseDto>(okResult.Value);
            Assert.Equal(1, returnedProduct.Id);
            Assert.Equal("Test Product", returnedProduct.Name);
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

        [Fact]
        public async Task GetProduct_WithNonExistentId_ShouldReturnNotFound()
        {
            // Arrange
            _mockProductService.Setup(s => s.GetProductByIdAsync(999)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.GetProduct(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("找不到 ID 為 999 的商品", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateProduct_WithValidData_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Description = "New Description",
                Price = 299.99m,
                Category = "電子產品"
            };

            var createdProduct = new Product
            {
                Id = 4,
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
            Assert.Equal(4, returnedProduct.Id);
            Assert.Equal("New Product", returnedProduct.Name);
            Assert.Equal("GetProduct", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task CreateProduct_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = "", // 無效的名稱
                Description = "Description",
                Price = 100,
                Category = "電子產品"
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

        [Fact]
        public async Task UpdateProduct_WithValidData_ShouldReturnOkWithUpdatedProduct()
        {
            // Arrange
            var updateDto = new UpdateProductDto
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 199.99m,
                Category = "電子產品",
                IsActive = true
            };

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Original Product",
                Description = "Original Description",
                Price = 100,
                Category = "電子產品",
                CreatedDate = DateTime.Now.AddDays(-1),
                IsActive = true
            };

            var updatedProduct = new Product
            {
                Id = 1,
                Name = updateDto.Name,
                Description = updateDto.Description,
                Price = updateDto.Price,
                Category = updateDto.Category,
                CreatedDate = existingProduct.CreatedDate,
                IsActive = updateDto.IsActive
            };

            _mockProductService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductService.Setup(s => s.UpdateProductAsync(It.IsAny<Product>())).ReturnsAsync(updatedProduct);

            // Act
            var result = await _controller.UpdateProduct(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductResponseDto>(okResult.Value);
            Assert.Equal("Updated Product", returnedProduct.Name);
            Assert.Equal("Updated Description", returnedProduct.Description);
        }

        [Fact]
        public async Task UpdateProduct_WithMismatchedId_ShouldReturnBadRequest()
        {
            // Arrange
            var updateDto = new UpdateProductDto { Id = 2 };

            // Act
            var result = await _controller.UpdateProduct(1, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("路徑中的 ID 與請求體中的 ID 不匹配", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_WithNonExistentProduct_ShouldReturnNotFound()
        {
            // Arrange
            var updateDto = new UpdateProductDto
            {
                Id = 999,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 199.99m,
                Category = "電子產品",
                IsActive = true
            };

            _mockProductService.Setup(s => s.GetProductByIdAsync(999)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.UpdateProduct(999, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("找不到 ID 為 999 的商品", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _mockProductService.Setup(s => s.ProductExistsAsync(1)).ReturnsAsync(true);
            _mockProductService.Setup(s => s.DeleteProductAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(1);

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

        [Fact]
        public async Task DeleteProduct_WithNonExistentId_ShouldReturnNotFound()
        {
            // Arrange
            _mockProductService.Setup(s => s.ProductExistsAsync(999)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("找不到 ID 為 999 的商品", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteProduct_WhenDeleteFails_ShouldReturnBadRequest()
        {
            // Arrange
            _mockProductService.Setup(s => s.ProductExistsAsync(1)).ReturnsAsync(true);
            _mockProductService.Setup(s => s.DeleteProductAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("刪除商品失敗", badRequestResult.Value);
        }
    }
}
