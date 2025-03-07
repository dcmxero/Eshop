using Application.DTOs;
using Application.Services;
using Infrastructure.Exceptions;
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
    /// <response code="400">If the request is invalid.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("all")]
    [SwaggerOperation(Summary = "Get all products", Description = "Retrieves all products. Pagination is not supported in v1.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetProductsAsync()
    {
        List<ProductDto> products = await productService.GetAllProductsAsync();
        return Ok(products);
    }

    /// <summary>
    /// Retrieves all active products.
    /// </summary>
    /// <returns>A list of all active products.</returns>
    /// <response code="200">Returns a list of active products.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("active")]
    [SwaggerOperation(Summary = "Get all active products", Description = "Retrieves all active products.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetActiveProductsAsync()
    {
        List<ProductDto> products = await productService.GetAllActiveProductsAsync();
        return Ok(products);
    }

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product with the specified ID.</returns>
    /// <response code="200">Returns the product with the specified ID.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the product with the specified ID is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get product by ID", Description = "Retrieves a product by its ID.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        try
        {
            ProductDto product = await productService.GetProductByIdAsync(id);
            return Ok(product);
        }
        catch (ProductNotFoundException)
        {
            return NotFound("Product not found.");
        }
        catch (Exception)
        {
            return StatusCode(500, $"An internal server error occurred.");
        }
    }

    /// <summary>
    /// Updates the description of a product.
    /// </summary>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="request">The request body containing the new product description.</param>
    /// <returns>A message indicating the success of the update.</returns>
    /// <response code="200">Product description updated successfully.</response>
    /// <response code="400">If the product ID is invalid.</response>
    /// <response code="404">If the product with the specified ID is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPut("{id}/description")]
    [SwaggerOperation(Summary = "Update a product description", Description = "Updates product description.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateProductDescriptionAsync(int id, [FromBody] UpdateProductDto request)
    {
        if (id == default)
        {
            return BadRequest("Product ID must be provided.");
        }

        try
        {
            await productService.UpdateProductDescriptionAsync(id, request.Description);
        }
        catch (ProductNotFoundException)
        {
            return NotFound("Product not found.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error occurred.");
        }

        return Ok("Product description updated successfully.");
    }

    /// <summary>
    /// Activates a product by setting its IsActive property to true.
    /// </summary>
    /// <param name="productId">The ID of the product to activate.</param>
    /// <returns>A message indicating the success of the activation.</returns>
    /// <response code="200">Product activated successfully.</response>
    /// <response code="404">If the product with the specified ID is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPatch("{productId}/activate")]
    [SwaggerOperation(Summary = "Activates a product", Description = "Sets the IsActive property of the product to true.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> ActivateProductAsync(int productId)
    {
        try
        {
            await productService.SetIsActiveAsync(productId, true);
        }
        catch (ProductNotFoundException)
        {
            return NotFound("Product not found.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error occurred.");
        }

        return Ok("Product activated successfully.");
    }

    /// <summary>
    /// Deactivates a product by setting its IsActive property to false.
    /// </summary>
    /// <param name="productId">The ID of the product to deactivate.</param>
    /// <returns>A message indicating the success of the deactivation.</returns>
    /// <response code="200">Product deactivated successfully.</response>
    /// <response code="404">If the product with the specified ID is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPatch("{productId}/deactivate")]
    [SwaggerOperation(Summary = "Deactivates a product", Description = "Sets the IsActive property of the product to false.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> DeactivateProductAsync(int productId)
    {
        try
        {
            await productService.SetIsActiveAsync(productId, false);
        }
        catch (ProductNotFoundException)
        {
            return NotFound("Product not found.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error occurred.");
        }

        return Ok("Product deactivated successfully.");
    }
}