using Domain.Models.ProductManagement;

namespace Infrastructure.Repositories.ProductManagement;

/// <summary>
/// Defines the contract for the product repository.
/// </summary>
public interface IProductRepository : IGenericRepository<Product>
{
    /// <summary>
    /// Sets the active status of a product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="isActive">The new active status of the product.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. Returns true if the product was successfully updated, otherwise false.</returns>
    Task<bool> SetIsActiveAsync(int productId, bool isActive, CancellationToken cancellationToken = default);
}