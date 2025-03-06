using Application.DTOs;
using Application.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers.v1;
using Xunit;

namespace Application.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> mockProductService;
        private readonly ProductsController controller;

        public ProductsControllerTests()
        {
            mockProductService = new Mock<IProductService>();
            controller = new ProductsController(mockProductService.Object);
        }

        [Fact]
        public async Task GetProductsV1_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            List<ProductDto> products =
            [
                new() { Id = 1, Name = "Product 1", ImgUri = "image1.jpg", Price = 9.99M, Description = "Description 1" },
                new() { Id = 2, Name = "Product 2", ImgUri = "image2.jpg", Price = 19.99M, Description = "Description 2" }
            ];
            mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            IActionResult result = await controller.GetProducts();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<ProductDto> returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task UpdateDescriptionDetail_ReturnsNoContent_WhenProductUpdatedSuccessfully()
        {
            // Arrange
            UpdateProductDto productDto = new() { Id = 1, Description = "Updated Description" };
            mockProductService.Setup(service => service.UpdateProductDescriptionAsync(productDto));

            // Act
            IActionResult result = await controller.UpdateProductDescriptionAsync(productDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateDescriptionDetail_ReturnsBadRequest_WhenProductIdIsDefault()
        {
            // Arrange
            UpdateProductDto productDto = new() { Id = default, Description = "Updated Description" };

            // Act
            IActionResult result = await controller.UpdateProductDescriptionAsync(productDto);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Product ID must be provided and cannot be the default value.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateDescriptionDetail_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            UpdateProductDto productDto = new() { Id = 1, Description = "Updated Description" };
            mockProductService.Setup(service => service.UpdateProductDescriptionAsync(productDto)).ThrowsAsync(new ProductNotFoundException());

            // Act
            IActionResult result = await controller.UpdateProductDescriptionAsync(productDto);

            // Assert
            NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateDescriptionDetail_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            UpdateProductDto productDto = new() { Id = 1, Description = "Updated Description" };
            mockProductService.Setup(service => service.UpdateProductDescriptionAsync(productDto)).ThrowsAsync(new Exception());

            // Act
            IActionResult result = await controller.UpdateProductDescriptionAsync(productDto);

            // Assert
            ObjectResult statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An internal server error occurred.", statusCodeResult.Value);
        }
    }
}
