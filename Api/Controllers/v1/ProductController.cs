using Application.DTOs;
using Application.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService productService = productService;

        [HttpGet]
        [SwaggerOperation(Summary = "Get all products", Description = "Retrieves all products. Pagination is not supported in v1.")]
        public async Task<IActionResult> GetProducts()
        {
            List<ProductDto> products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all active products", Description = "Retrieves all active products.")]
        public async Task<IActionResult> GetActiveProducts()
        {
            List<ProductDto> products = await productService.GetAllActiveProductsAsync();
            return Ok(products);
        }

        [HttpPut()]
        [SwaggerOperation(Summary = "Update a product description", Description = "Updates product description.")]
        public async Task<IActionResult> UpdateProductDescriptionAsync(UpdateProductDto productDto)
        {
            if (productDto.Id == default)
            {
                return BadRequest("Product ID must be provided and cannot be the default value.");
            }

            try
            {
                await productService.UpdateProductDescriptionAsync(productDto);
            }
            catch (ProductNotFoundException)
            {
                return BadRequest("Product not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal server error occurred.");
            }

            return NoContent();
        }

        /// <summary>
        /// Activates a product (sets IsActive to true).
        /// </summary>
        /// <param name="productId">The ID of the product to activate.</param>
        /// <returns>No Content if the product is activated successfully, otherwise a NotFound status.</returns>
        [HttpPost("activate/{productId}")]
        [SwaggerOperation(Summary = "Activates a product", Description = "Sets the IsActive property of the product to true.")]
        public async Task<IActionResult> ActivateProduct(int productId)
        {
            try
            {
                await productService.SetIsActiveAsync(productId, true);
            }
            catch (ProductNotFoundException)
            {
                return BadRequest("Product not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal server error occurred.");
            }

            return NoContent();
        }

        /// <summary>
        /// Deactivates a product (sets IsActive to false).
        /// </summary>
        /// <param name="productId">The ID of the product to deactivate.</param>
        /// <returns>No Content if the product is deactivated successfully, otherwise a NotFound status.</returns>
        [HttpPost("deactivate/{productId}")]
        [SwaggerOperation(Summary = "Deactivates a product", Description = "Sets the IsActive property of the product to false.")]
        public async Task<IActionResult> DeactivateProduct(int productId)
        {
            try
            {
                await productService.SetIsActiveAsync(productId, false);
            }
            catch (ProductNotFoundException)
            {
                return BadRequest("Product not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal server error occurred.");
            }

            return NoContent();
        }
    }
}
