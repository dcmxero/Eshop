using Microsoft.EntityFrameworkCore;

namespace DTOs.Common;

public class PaginatedList<T>(List<T> items, int totalCount, int pageIndex, int pageSize)
{
    /// <summary>
    /// Gets the list of items for the current page.
    /// </summary>
    public List<T> Data { get; set; } = items;

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    public int TotalCount { get; set; } = totalCount;

    /// <summary>
    /// Gets the index of the current page.
    /// </summary>
    public int PageIndex { get; set; } = pageIndex;

    /// <summary>
    /// Gets the total number of pages available.
    /// </summary>
    public int TotalPages { get; set; } = (int)Math.Ceiling(totalCount / (double)pageSize);

    /// <summary>
    /// Indicates if there is a previous page.
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// Indicates if there is a next page.
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;

    /// <summary>
    /// Creates a PaginatedList from a queryable source, applying pagination.
    /// </summary>
    /// <param name="source">The queryable source to paginate.</param>
    /// <param name="totalCount">The total number of items in the source.</param>
    /// <param name="pageIndex">The page index to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A PaginatedList of the requested items.</returns>
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int totalCount, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        List<T> items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken: cancellationToken);
        return new PaginatedList<T>(items, totalCount, pageIndex, pageSize);
    }
}
