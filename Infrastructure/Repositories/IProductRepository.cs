using Domain.Models;

namespace Infrastructure.Repositories;

/// <summary>
/// Defines the contract for the product repository.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves all products from the repository.
    /// </summary>
    /// <returns>A list of all products.</returns>
    Task<List<Product>> GetAllProductsAsync();

    /// <summary>
    /// Retrieves all active products from the repository.
    /// </summary>
    /// <returns>A list of all active products.</returns>
    Task<List<Product>> GetAllActiveProductsAsync();

    /// <summary>
    /// Retrieves products from the repository with pagination.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <returns>A list of products for the specified page and size.</returns>
    Task<List<Product>> GetProductsAsync(int page, int pageSize);

    /// <summary>
    /// Retrieves active products from the repository with pagination.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <returns>A list of active products for the specified page and size.</returns>
    Task<List<Product>> GetActiveProductsAsync(int page, int pageSize);

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product with the specified ID, or null if not found.</returns>
    Task<Product> GetByIdAsync(int id);

    /// <summary>
    /// Updates the details of a product in the repository.
    /// </summary>
    /// <param name="product">The product to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(Product product);

    /// <summary>
    /// Sets the active status of a product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="isActive">The new active status of the product.</param>
    /// <returns>A task representing the asynchronous operation. Returns true if the product was successfully updated, otherwise false.</returns>
    Task<bool> SetIsActiveAsync(int productId, bool isActive);
}
