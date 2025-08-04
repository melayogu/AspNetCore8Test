using Microsoft.AspNetCore.Mvc;
using AspNetCore8Test.Models;
using AspNetCore8Test.Models.DTOs;
using AspNetCore8Test.Services;
using FluentValidation;

namespace AspNetCore8Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;

        public ProductsController(
            IProductService productService,
            IValidator<CreateProductDto> createValidator,
            IValidator<UpdateProductDto> updateValidator)
        {
            _productService = productService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            var response = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Category = p.Category,
                CreatedDate = p.CreatedDate,
                IsActive = p.IsActive
            });

            return Ok(response);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("商品 ID 必須大於 0");
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"找不到 ID 為 {id} 的商品");
            }

            var response = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                CreatedDate = product.CreatedDate,
                IsActive = product.IsActive
            };

            return Ok(response);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct(CreateProductDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                Category = createDto.Category
            };

            var createdProduct = await _productService.CreateProductAsync(product);
            
            var response = new ProductResponseDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                Category = createdProduct.Category,
                CreatedDate = createdProduct.CreatedDate,
                IsActive = createdProduct.IsActive
            };

            return CreatedAtAction(nameof(GetProduct), new { id = response.Id }, response);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct(int id, UpdateProductDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("路徑中的 ID 與請求體中的 ID 不匹配");
            }

            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound($"找不到 ID 為 {id} 的商品");
            }

            var product = new Product
            {
                Id = updateDto.Id,
                Name = updateDto.Name,
                Description = updateDto.Description,
                Price = updateDto.Price,
                Category = updateDto.Category,
                IsActive = updateDto.IsActive,
                CreatedDate = existingProduct.CreatedDate
            };

            var updatedProduct = await _productService.UpdateProductAsync(product);
            if (updatedProduct == null)
            {
                return NotFound($"更新失敗：找不到 ID 為 {id} 的商品");
            }

            var response = new ProductResponseDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price,
                Category = updatedProduct.Category,
                CreatedDate = updatedProduct.CreatedDate,
                IsActive = updatedProduct.IsActive
            };

            return Ok(response);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("商品 ID 必須大於 0");
            }

            var exists = await _productService.ProductExistsAsync(id);
            if (!exists)
            {
                return NotFound($"找不到 ID 為 {id} 的商品");
            }

            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
            {
                return BadRequest("刪除商品失敗");
            }

            return NoContent();
        }
    }
}
