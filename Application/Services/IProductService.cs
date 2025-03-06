using Application.DTOs;

namespace Application.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<List<ProductDto>> GetAllActiveProductsAsync();
    Task<List<ProductDto>> GetProductsAsync(int page, int pageSize);

    Task<List<ProductDto>> GetActiveProductsAsync(int page, int pageSize);
    Task UpdateProductDescriptionAsync(int productId, string? description);
    Task SetIsActiveAsync(int productId, bool isActive);
}
