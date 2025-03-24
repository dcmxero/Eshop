using DTOs.Common;
using DTOs.Product;
using MediatR;

namespace Application.Features.Products;

/// <summary>
/// Represents a request to retrieve a paginated list of active products.
/// </summary>
/// <remarks>
/// This request is used to fetch products that are marked as active, with support for pagination based on the specified page number and page size.
/// </remarks>
/// <param name="Page">The page number to retrieve.</param>
/// <param name="PageSize">The number of products per page.</param>
/// <returns>A paginated list of active products.</returns>
public class GetActiveProductsRequest : IRequest<PaginatedList<ProductDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}