using Application.DTOs;
using Application.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/products")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductsController(ProductService productService)
        {
            this.productService = productService;
        }

        // GET: api/v1/products (Get All Products - v1)
        // GET: api/v2/products (Get Products with Pagination - v2)
        [HttpGet]
        [MapToApiVersion("1.0")]
        [SwaggerOperation(Summary = "Get products", Description = "Retrieves all products. Pagination is not supported in v1.")]
        public async Task<IActionResult> GetProductsV1()
        {
            // v1: Retrieve all products without pagination.
            List<ProductDto> products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/v2/products (Get Products with Pagination - v2)
        [HttpGet]
        [MapToApiVersion("2.0")]
        [SwaggerOperation(Summary = "Get products", Description = "Retrieves products with pagination.")]
        public async Task<IActionResult> GetProductsV2(int page = 1, int pageSize = 10)
        {
            // v2: Retrieve products with pagination.
            List<ProductDto> pagedProducts = await productService.GetProductsAsync(page, pageSize);
            return Ok(pagedProducts);
        }

        // PUT: api/v1/products/{id} (Update Product Description)
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a product description", Description = "Updates product description.")]
        public async Task<IActionResult> UpdateDescriptionDetail(int id, UpdateProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest("Product ID in URL does not match the ID in the request body.");
            }

            try
            {
                await productService.UpdateProductDescriptionAsync(productDto);
            }
            catch (ProductNotFoundException)
            {
                return NotFound("Product not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal server error occurred.");
            }

            return NoContent();
        }
    }
}
