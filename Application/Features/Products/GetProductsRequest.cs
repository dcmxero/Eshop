using DTOs.Common;
using DTOs.Product;
using MediatR;

namespace Application.Features.Products;

/// <summary>
/// Represents a request to retrieve a paginated list of products.
/// </summary>
/// <remarks>
/// This request is used to fetch products with pagination support, based on the specified page number and page size.
/// </remarks>
/// <param name="Page">The page number to retrieve.</param>
/// <param name="PageSize">The number of products per page.</param>
/// <returns>A paginated list of products.</returns>
public class GetProductsRequest : IRequest<PaginatedList<ProductDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}