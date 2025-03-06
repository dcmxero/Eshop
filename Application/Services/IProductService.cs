using Application.DTOs;

namespace Application.Services;

/// <summary>
/// Defines the contract for the product service.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Retrieves all products from the service.
    /// </summary>
    /// <returns>A list of all products.</returns>
    Task<List<ProductDto>> GetAllProductsAsync();

    /// <summary>
    /// Retrieves all active products from the service.
    /// </summary>
    /// <returns>A list of all active products.</returns>
    Task<List<ProductDto>> GetAllActiveProductsAsync();

    /// <summary>
    /// Retrieves products from the service with pagination.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <returns>A list of products for the specified page and size.</returns>
    Task<List<ProductDto>> GetProductsAsync(int page, int pageSize);

    /// <summary>
    /// Retrieves active products from the service with pagination.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of active products per page.</param>
    /// <returns>A list of active products for the specified page and size.</returns>
    Task<List<ProductDto>> GetActiveProductsAsync(int page, int pageSize);

    /// <summary>
    /// Updates the description of a product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="description">The new description for the product.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateProductDescriptionAsync(int productId, string? description);

    /// <summary>
    /// Sets the active status of a product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="isActive">The new active status of the product.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetIsActiveAsync(int productId, bool isActive);
}
