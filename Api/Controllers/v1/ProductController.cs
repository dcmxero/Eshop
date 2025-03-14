using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService productService = productService;

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <remarks>Pagination is not supported in v1.</remarks>
    /// <returns>A list of all products.</returns>
    /// <response code="200">Returns a list of products.</response>
    [HttpGet("all")]
    [SwaggerOperation(Summary = "Get all products", Description = "Retrieves all products. Pagination is not supported in v1.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    public async Task<IActionResult> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        List<ProductDto> products = await productService.GetAllProductsAsync(cancellationToken);
        return Ok(products);
    }

    /// <summary>
    /// Retrieves all active products.
    /// </summary>
    /// <returns>A list of all active products.</returns>
    /// <response code="200">Returns a list of active products.</response>
    [HttpGet("active")]
    [SwaggerOperation(Summary = "Get all active products", Description = "Retrieves all active products.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    public async Task<IActionResult> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        List<ProductDto> products = await productService.GetAllActiveProductsAsync(cancellationToken);
        return Ok(products);
    }

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product with the specified ID.</returns>
    /// <response code="200">Returns the product with the specified ID.</response>
    /// <response code="404">If the product with the specified ID is not found.</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get product by ID", Description = "Retrieves a product by its ID.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        ProductDto? product = await productService.GetProductByIdAsync(id, cancellationToken);
        return product == null ? NotFound("Product not found.") : Ok(product);
    }

    /// <summary>
    /// Updates the description of a product.
    /// </summary>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="request">The request body containing the new product description.</param>
    /// <returns>A message indicating the success of the update.</returns>
    /// <response code="200">Product description updated successfully.</response>
    /// <response code="404">If the product with the specified ID is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPut("{id}/description")]
    [SwaggerOperation(Summary = "Update a product description", Description = "Updates product description.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateProductDescriptionAsync(int id, [FromBody] UpdateProductDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            bool updateSucceeded = await productService.UpdateProductDescriptionAsync(id, request.Description, cancellationToken);
            return !updateSucceeded ? NotFound("Product not found.") : Ok("Product description updated successfully.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error occurred.");
        }
    }

    /// <summary>
    /// Activates a product by setting its IsActive property to true.
    /// </summary>
    /// <param name="productId">The ID of the product to activate.</param>
    /// <returns>A message indicating the success of the activation.</returns>
    /// <response code="200">Product activated successfully.</response>
    /// <response code="404">If the product with the specified ID is not found.</response>
    [HttpPatch("{productId}/activate")]
    [SwaggerOperation(Summary = "Activates a product", Description = "Sets the IsActive property of the product to true.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> ActivateProductAsync(int productId, CancellationToken cancellationToken = default)
    {
        try
        {
            bool activationSucceeded = await productService.SetIsActiveAsync(productId, true, cancellationToken);
            return !activationSucceeded ? NotFound("Product activation was not successful.") : Ok("Product activated successfully.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error occurred.");
        }
    }

    /// <summary>
    /// Deactivates a product by setting its IsActive property to false.
    /// </summary>
    /// <param name="productId">The ID of the product to deactivate.</param>
    /// <returns>A message indicating the success of the deactivation.</returns>
    /// <response code="200">Product deactivated successfully.</response>
    /// <response code="404">If the product with the specified ID is not found.</response>
    [HttpPatch("{productId}/deactivate")]
    [SwaggerOperation(Summary = "Deactivates a product", Description = "Sets the IsActive property of the product to false.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> DeactivateProductAsync(int productId, CancellationToken cancellationToken = default)
    {
        try
        {
            bool deactivationSucceeded = await productService.SetIsActiveAsync(productId, false, cancellationToken);
            return !deactivationSucceeded ? NotFound("Product deactivation was not successful.") : Ok("Product deactivated successfully.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error occurred.");
        }
    }
}