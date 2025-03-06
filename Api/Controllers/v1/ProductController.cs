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

    [HttpGet]
    [SwaggerOperation(Summary = "Get all products", Description = "Retrieves all products. Pagination is not supported in v1.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetProducts()
    {
        List<ProductDto> products = await productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("active")]
    [SwaggerOperation(Summary = "Get all active products", Description = "Retrieves all active products.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetActiveProducts()
    {
        List<ProductDto> products = await productService.GetAllActiveProductsAsync();
        return Ok(products);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a product description", Description = "Updates product description.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // For successful update with message
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))] // For bad request (e.g., invalid productId)
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))] // For product not found
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))] // For internal server errors
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

    [HttpPatch("{productId}/activate")]
    [SwaggerOperation(Summary = "Activates a product", Description = "Sets the IsActive property of the product to true.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // For successful activation with message
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))] // For product not found
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))] // For internal server errors
    public async Task<IActionResult> ActivateProduct(int productId)
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

    [HttpPatch("{productId}/deactivate")]
    [SwaggerOperation(Summary = "Deactivates a product", Description = "Sets the IsActive property of the product to false.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // For successful deactivation with message
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))] // For product not found
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))] // For internal server errors
    public async Task<IActionResult> DeactivateProduct(int productId)
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
