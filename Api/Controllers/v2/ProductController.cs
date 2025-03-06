using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v2/[controller]")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService productService = productService;

        [HttpGet]
        [SwaggerOperation(Summary = "Get products with pagination", Description = "Retrieves products with pagination.")]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10)
        {
            List<ProductDto> pagedProducts = await productService.GetProductsAsync(page, pageSize);
            return Ok(pagedProducts);
        }
    }
}
