using Application.Services;
using DTOs.Common;
using DTOs.Product;
using MediatR;

namespace Application.Features.Products;

/// <summary>
/// Handler for processing the GetActiveProductsRequest and retrieving a paginated list of active products.
/// </summary>
/// <remarks>
/// This handler uses the IProductService to retrieve only active products based on the page number and page size specified in the request.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the GetActiveProductsHandler class.
/// </remarks>
/// <param name="productService">The product service used to fetch active products.</param>
public class GetActiveProductsHandler(IProductService productService) : IRequestHandler<GetActiveProductsRequest, PaginatedList<ProductDto>>
{
    private readonly IProductService productService = productService;

    /// <summary>
    /// Handles the GetActiveProductsRequest and returns a paginated list of active products.
    /// </summary>
    /// <param name="request">The GetActiveProductsRequest containing the page number and page size.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of type PaginatedList<ProductDto>.</returns>
    public async Task<PaginatedList<ProductDto>> Handle(GetActiveProductsRequest request, CancellationToken cancellationToken)
    {
        return await productService.GetActiveProductsAsync(request.Page, request.PageSize, cancellationToken);
    }
}