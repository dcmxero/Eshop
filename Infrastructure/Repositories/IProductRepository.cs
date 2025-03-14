using Domain.Models;
using DTOs.Common;

namespace Infrastructure.Repositories;

/// <summary>
/// Defines the contract for the product repository.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves all products from the repository.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all products.</returns>
    Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all active products from the repository.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all active products.</returns>
    Task<List<Product>> GetAllActiveProductsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of products from the repository.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a `DataResultDto<Product>` with the paginated products and total count.</returns>
    Task<DataResultDto<Product>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of active products from the repository.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a `DataResultDto<Product>` with the paginated active products and total count.</returns>
    Task<DataResultDto<Product>> GetActiveProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="productId">The ID of the product to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product with the specified ID, or null if not found.</returns>
    Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the details of a product in the repository.
    /// </summary>
    /// <param name="product">The product to update.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the active status of a product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="isActive">The new active status of the product.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. Returns true if the product was successfully updated, otherwise false.</returns>
    Task<bool> SetIsActiveAsync(int productId, bool isActive, CancellationToken cancellationToken = default);
}