using Application.DTOs;
using Application.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers.v1;
using Xunit;

namespace WebApi.Tests;

public class ProductsControllerV1Tests
{
    private readonly Mock<IProductService> mockProductService;
    private readonly ProductsController controller;

    public ProductsControllerV1Tests()
    {
        mockProductService = new Mock<IProductService>();
        controller = new ProductsController(mockProductService.Object);
    }

    #region Get products

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
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
    public async Task GetActiveProductsV1_ReturnsOkResult_WithFilteredActiveProducts()
    {
        // Arrange
        List<ProductDto> activeProducts =
        [
            new() { Id = 1, Name = "Product 1", ImgUri = "active1.jpg", Price = 15.99M, Description = "Description 1" },
            new() { Id = 2, Name = "Product 2", ImgUri = "active2.jpg", Price = 25.99M, Description = "Description 2" }
        ];

        // Mock service to return only the active products (assuming it's already filtered in the service layer)
        mockProductService.Setup(service => service.GetAllActiveProductsAsync()).ReturnsAsync(activeProducts);

        // Act
        IActionResult result = await controller.GetActiveProducts();

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        List<ProductDto> returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);

        // Ensure the result contains exactly the products expected (simulating only active products)
        Assert.Equal(2, returnValue.Count); // We expect 2 active products in this case
        Assert.Equal(activeProducts[0].Id, returnValue[0].Id);
        Assert.Equal(activeProducts[1].Id, returnValue[1].Id);
    }

    #endregion

    #region Update product description

    [Fact]
    public async Task UpdateProductDescription_ReturnsNoContent_WhenProductUpdatedSuccessfully()
    {
        // Arrange
        UpdateProductDto request = new() { Description = "Updated Description" };
        int id = 1;
        mockProductService.Setup(service => service.UpdateProductDescriptionAsync(id, request.Description));

        // Act
        IActionResult result = await controller.UpdateProductDescriptionAsync(id, request);

        // Assert
        OkObjectResult notFoundResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Product description updated successfully.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateProductDescription_ReturnsBadRequest_WhenProductIdIsDefault()
    {
        // Arrange
        UpdateProductDto request = new() { Description = "Updated Description" };
        int id = default;

        // Act
        IActionResult result = await controller.UpdateProductDescriptionAsync(id, request);

        // Assert
        // Assert
        BadRequestObjectResult notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Product ID must be provided.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateProductDescription_ReturnsNotFound_WhenProductNotFound()
    {
        // Arrange
        UpdateProductDto request = new() { Description = "Updated Description" };
        int id = 1;
        mockProductService.Setup(service => service.UpdateProductDescriptionAsync(id, request.Description)).ThrowsAsync(new ProductNotFoundException());

        // Act
        IActionResult result = await controller.UpdateProductDescriptionAsync(id, request);

        // Assert
        // Assert
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Product not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateProductDescription_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        UpdateProductDto request = new() { Description = "Updated Description" };
        int id = 1;
        mockProductService.Setup(service => service.UpdateProductDescriptionAsync(id, request.Description)).ThrowsAsync(new Exception());

        // Act
        IActionResult result = await controller.UpdateProductDescriptionAsync(id, request);

        // Assert
        ObjectResult statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An internal server error occurred.", statusCodeResult.Value);
    }

    #endregion

    #region Activate product

    [Fact]
    public async Task ActivateProduct_ReturnsNoContent_WhenProductActivatedSuccessfully()
    {
        // Arrange
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, true));

        // Act
        IActionResult result = await controller.ActivateProduct(productId);

        // Assert
        OkObjectResult notFoundResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Product activated successfully.", notFoundResult.Value);
    }

    [Fact]
    public async Task ActivateProduct_ReturnsBadRequest_WhenProductNotFound()
    {
        // Arrange
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, true)).ThrowsAsync(new ProductNotFoundException());

        // Act
        IActionResult result = await controller.ActivateProduct(productId);

        // Assert
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Product not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task ActivateProduct_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, true)).ThrowsAsync(new Exception());

        // Act
        IActionResult result = await controller.ActivateProduct(productId);

        // Assert
        ObjectResult statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An internal server error occurred.", statusCodeResult.Value);
    }

    #endregion

    #region Deactivate product

    [Fact]
    public async Task DeactivateProduct_ReturnsNoContent_WhenProductDeactivatedSuccessfully()
    {
        // Arrange
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, false));

        // Act
        IActionResult result = await controller.DeactivateProduct(productId);

        // Assert
        OkObjectResult notFoundResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Product deactivated successfully.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeactivateProduct_ReturnsBadRequest_WhenProductNotFound()
    {
        // Arrange
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, false)).ThrowsAsync(new ProductNotFoundException());

        // Act
        IActionResult result = await controller.DeactivateProduct(productId);

        // Assert
        // Assert
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Product not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeactivateProduct_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, false)).ThrowsAsync(new Exception());

        // Act
        IActionResult result = await controller.DeactivateProduct(productId);

        // Assert
        ObjectResult statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An internal server error occurred.", statusCodeResult.Value);
    }

    #endregion
}
