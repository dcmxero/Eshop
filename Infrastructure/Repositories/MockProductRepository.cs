using Domain.Models;
using Infrastructure.Exceptions;

namespace Infrastructure.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> mockProducts;

        public MockProductRepository()
        {
            mockProducts = GenerateMockData();
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            return Task.FromResult(mockProducts);
        }

        public Task<List<Product>> GetProductsAsync(int page, int pageSize)
        {
            List<Product> products = [.. mockProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)];
            return Task.FromResult(products);
        }

        public Task<Product> GetByIdAsync(int id)
        {
            Product? product = mockProducts.FirstOrDefault(p => p.Id == id);
            return product != null ? Task.FromResult(product) : throw new ProductNotFoundException($"Product with ID {id} was not found.");
        }

        public Task UpdateAsync(Product product)
        {
            Product? existingProduct = mockProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return Task.FromResult(false);
            }

            // Update the product properties
            existingProduct.Name = product.Name;
            existingProduct.ImgUri = product.ImgUri;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;

            return Task.FromResult(true);
        }

        private static List<Product> GenerateMockData()
        {
            return
            [
                new Product { Id = 1, Name = "Product 1", ImgUri = "image1.jpg", Price = 9.99M, Description = "Description 1" },
                new Product { Id = 2, Name = "Product 2", ImgUri = "image2.jpg", Price = 19.99M, Description = "Description 2" },
                new Product { Id = 3, Name = "Product 3", ImgUri = "image3.jpg", Price = 29.99M, Description = "Description 3" },
                new Product { Id = 4, Name = "Product 4", ImgUri = "image4.jpg", Price = 49.99M, Description = "Description 4" },
                new Product { Id = 5, Name = "Product 5", ImgUri = "image5.jpg", Price = 99.99M, Description = "Description 5" }
            ];
        }
    }
}
