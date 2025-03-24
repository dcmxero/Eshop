namespace Infrastructure.UnitOfWork;

/// <summary>
/// Defines a contract for completing transactions in a unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Asynchronously commits all changes made during the unit of work.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CompleteAsync(CancellationToken cancellationToken = default);
}