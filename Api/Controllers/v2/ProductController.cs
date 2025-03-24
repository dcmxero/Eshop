using Application.Features.Products;
using DTOs.Common;
using DTOs.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    /// <summary>
    /// Retrieves products with pagination.
    /// </summary>
    /// <param name="page">The page number (must be 1 or greater).</param>
    /// <param name="pageSize">The number of products per page (must be 1 or greater).</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A paged list of products with pagination info.</returns>
    [HttpGet]
    [Route("all")]
    [SwaggerOperation(Summary = "Get products with pagination", Description = "Retrieves products with pagination.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetProductsV2Async(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (page < 1)
        {
            return BadRequest("Page number must be 1 or greater.");
        }

        if (pageSize < 1)
        {
            return BadRequest("Page size must be 1 or greater.");
        }

        PaginatedList<ProductDto> result = await mediator.Send(new GetProductsRequest { Page = page, PageSize = pageSize }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves active products with pagination.
    /// </summary>
    /// <param name="page">The page number (must be 1 or greater).</param>
    /// <param name="pageSize">The number of products per page (must be 1 or greater).</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A paged list of active products with pagination info.</returns>
    [HttpGet]
    [Route("active")]
    [SwaggerOperation(Summary = "Get active products with pagination", Description = "Retrieves active products with pagination.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetActiveProductsV2Async(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (page < 1)
        {
            return BadRequest("Page number must be 1 or greater.");
        }

        if (pageSize < 1)
        {
            return BadRequest("Page size must be 1 or greater.");
        }

        PaginatedList<ProductDto> result = await mediator.Send(new GetActiveProductsRequest { Page = page, PageSize = pageSize }, cancellationToken);
        return Ok(result);
    }
}
