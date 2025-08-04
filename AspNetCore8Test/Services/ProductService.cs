using AspNetCore8Test.Models;

namespace AspNetCore8Test.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new();
        private int _nextId = 1;

        public ProductService()
        {
            // 初始化一些測試資料
            _products.AddRange(new[]
            {
                new Product { Id = _nextId++, Name = "iPhone 15", Description = "最新款 iPhone", Price = 32900, Category = "電子產品" },
                new Product { Id = _nextId++, Name = "Nike 運動鞋", Description = "舒適的運動鞋", Price = 2800, Category = "運動" },
                new Product { Id = _nextId++, Name = "程式設計書籍", Description = "學習程式設計的好書", Price = 450, Category = "書籍" }
            });
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return Task.FromResult(_products.Where(p => p.IsActive).AsEnumerable());
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            return Task.FromResult(product);
        }

        public Task<Product> CreateProductAsync(Product product)
        {
            product.Id = _nextId++;
            product.CreatedDate = DateTime.Now;
            product.IsActive = true;
            _products.Add(product);
            return Task.FromResult(product);
        }

        public Task<Product?> UpdateProductAsync(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
                return Task.FromResult<Product?>(null);

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
            existingProduct.IsActive = product.IsActive;

            return Task.FromResult<Product?>(existingProduct);
        }

        public Task<bool> DeleteProductAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return Task.FromResult(false);

            product.IsActive = false; // 軟刪除
            return Task.FromResult(true);
        }

        public Task<bool> ProductExistsAsync(int id)
        {
            var exists = _products.Any(p => p.Id == id && p.IsActive);
            return Task.FromResult(exists);
        }
    }
}
