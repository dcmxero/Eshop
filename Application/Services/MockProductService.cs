using Application.Mappers;
using DTOs.Common;
using DTOs.Product;

namespace Application.Services;

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

    public Task<List<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(products);
    }

    public Task<List<ProductDto>> GetAllActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(products.Where(p => p.IsActive).ToList());
    }

    public Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(products.FirstOrDefault(p => p.Id == productId));
    }

    public Task<PaginatedList<ProductDto>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new PaginatedList<ProductDto>([.. products.Skip((page - 1) * pageSize).Take(pageSize)], products.Count, page, pageSize));
    }

    public Task<PaginatedList<ProductDto>> GetActiveProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new PaginatedList<ProductDto>([.. products.Where(p => p.IsActive).Skip((page - 1) * pageSize).Take(pageSize)], products.Count, page, pageSize));
    }

    public Task<bool> UpdateProductDescriptionAsync(int productId, string? description, CancellationToken cancellationToken = default)
    {
        ProductDto? product = products.FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            return Task.FromResult(false);
        }

        product.Description = description;
        return Task.FromResult(true);
    }

    public Task<bool> SetIsActiveAsync(int productId, bool isActive, CancellationToken cancellationToken = default)
    {
        ProductDto? product = products.FirstOrDefault(p => p.Id == productId);

        if (product == null)
        {
            return Task.FromResult(false);
        }

        product.IsActive = isActive;
        return Task.FromResult(true);
    }    
}
