using Application.Services;
using DTOs.Common;
using DTOs.Product;
using MediatR;

namespace Application.Features.Products;

/// <summary>
/// Handler for processing the GetProductsRequest and retrieving a paginated list of products.
/// </summary>
/// <remarks>
/// This handler uses the IProductService to retrieve products based on the page number and page size specified in the request.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the GetProductsHandler class.
/// </remarks>
/// <param name="productService">The product service used to fetch products.</param>
public class GetProductsHandler(IProductService productService) : IRequestHandler<GetProductsRequest, PaginatedList<ProductDto>>
{
    private readonly IProductService productService = productService;

    /// <summary>
    /// Handles the GetProductsRequest and returns a paginated list of products.
    /// </summary>
    /// <param name="request">The GetProductsRequest containing the page number and page size.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of type PaginatedList<ProductDto>.</returns>
    public async Task<PaginatedList<ProductDto>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
    {
        return await productService.GetProductsAsync(request.Page, request.PageSize, cancellationToken);
    }
}