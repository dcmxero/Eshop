namespace Infrastructure.Repositories;

/// <summary>
/// Provides generic methods for accessing entities from a repository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to be managed by the repository.</typeparam>
public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <returns>An IQueryable collection of entities.</returns>
    IQueryable<TEntity> GetAll();

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation, with the entity as its result.</returns>
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}