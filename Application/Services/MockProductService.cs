namespace Application.Services;

using Application.DTOs;

public class MockProductService : IProductService
{
    private readonly List<ProductDto> products =
[
    new ProductDto { Id = 1, Name = "Product 1", ImgUri = "product1.jpg", Price = 10.99M, IsActive = true },
    new ProductDto { Id = 2, Name = "Product 2", ImgUri = "product2.jpg", Price = 12.99M, IsActive = false },
    new ProductDto { Id = 3, Name = "Product 3", ImgUri = "product3.jpg", Price = 14.99M, IsActive = true },
    new ProductDto { Id = 4, Name = "Product 4", ImgUri = "product4.jpg", Price = 16.99M, IsActive = true },
    new ProductDto { Id = 5, Name = "Product 5", ImgUri = "product5.jpg", Price = 18.99M, IsActive = false },
    new ProductDto { Id = 6, Name = "Product 6", ImgUri = "product6.jpg", Price = 20.99M, IsActive = true }
];

    public Task<List<ProductDto>> GetAllProductsAsync()
    {
        return Task.FromResult(products);
    }

    public Task<List<ProductDto>> GetAllActiveProductsAsync()
    {
        return Task.FromResult(products.Where(p => p.IsActive).ToList());
    }

    public Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<List<ProductDto>> GetProductsAsync(int page, int pageSize)
    {
        return Task.FromResult(products.Skip((page - 1) * pageSize).Take(pageSize).ToList());
    }

    public Task<List<ProductDto>> GetActiveProductsAsync(int page, int pageSize)
    {
        return Task.FromResult(products.Where(p => p.IsActive).Skip((page - 1) * pageSize).Take(pageSize).ToList());
    }

    public Task UpdateProductDescriptionAsync(int productId, string? description)
    {
        ProductDto? product = products.FirstOrDefault(p => p.Id == productId);

        if (product != null)
        {
            product.Description = description;
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    public Task SetIsActiveAsync(int productId, bool isActive)
    {
        ProductDto? product = products.FirstOrDefault(p => p.Id == productId);
        if (product != null)
        {
            product.IsActive = isActive;
        }
        return Task.CompletedTask;
    }
}
