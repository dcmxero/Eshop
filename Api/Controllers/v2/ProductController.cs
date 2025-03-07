using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v2/products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService productService = productService;

    /// <summary>
    /// Retrieves products with pagination.
    /// </summary>
    /// <param name="page">The page number (must be 1 or greater).</param>
    /// <param name="pageSize">The number of products per page (must be 1 or greater).</param>
    /// <returns>A paged list of products.</returns>
    [HttpGet("all")]
    [SwaggerOperation(Summary = "Get products with pagination", Description = "Retrieves products with pagination.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetProductsV2Async(int page = 1, int pageSize = 10)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be 1 or greater.");
        }

        List<ProductDto> pagedProducts = await productService.GetProductsAsync(page, pageSize);
        return Ok(pagedProducts);
    }

    /// <summary>
    /// Retrieves active products with pagination.
    /// </summary>
    /// <param name="page">The page number (must be 1 or greater).</param>
    /// <param name="pageSize">The number of products per page (must be 1 or greater).</param>
    /// <returns>A paged list of active products.</returns>
    [HttpGet("active")]
    [SwaggerOperation(Summary = "Get active products with pagination", Description = "Retrieves active products with pagination.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetActiveProductsV2Async(int page = 1, int pageSize = 10)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be 1 or greater.");
        }

        List<ProductDto> pagedProducts = await productService.GetActiveProductsAsync(page, pageSize);
        return Ok(pagedProducts);
    }
}
