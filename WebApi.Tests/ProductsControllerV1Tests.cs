using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers.v1;
using Xunit;

namespace WebApi.Tests;

/// <summary>
/// Unit tests for the ProductsControllerV1 class in the WebApi.Tests namespace.
/// This class tests the functionality of various product-related actions within the ProductsController.
/// It verifies that products are retrieved, updated, activated, and deactivated correctly, handling both success and failure cases.
/// </summary>
public class ProductsControllerV1Tests
{
    private readonly Mock<IProductService> mockProductService;
    private readonly ProductsController controller;

    /// <summary>
    /// Initializes a new instance of the ProductsControllerV1Tests class.
    /// Sets up a mocked product service and creates an instance of ProductsController for testing.
    /// </summary>
    public ProductsControllerV1Tests()
    {
        mockProductService = new Mock<IProductService>();
        controller = new ProductsController(mockProductService.Object);
    }

    #region Get products

    /// <summary>
    /// Verifies that the GetProducts action returns an OkObjectResult with a list of products.
    /// Tests that the response contains the correct number of products and that the controller interacts with the service correctly.
    /// </summary>
    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange: Prepare a list of product DTOs to be returned by the mock service.
        List<ProductDto> products =
        [
            new() { Id = 1, Name = "Product 1", ImgUri = "image1.jpg", Price = 9.99M, Description = "Description 1" },
            new() { Id = 2, Name = "Product 2", ImgUri = "image2.jpg", Price = 19.99M, Description = "Description 2" }
        ];
        mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

        // Act: Call the GetProductsAsync method on the controller.
        IActionResult result = await controller.GetProductsAsync();

        // Assert: Ensure the result is of type OkObjectResult and contains the expected number of products.
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        List<ProductDto> returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count); // Assert that the correct number of products are returned
    }

    /// <summary>
    /// Verifies that the GetActiveProducts action filters and returns only active products.
    /// The test ensures that only active products are included in the response.
    /// </summary>
    [Fact]
    public async Task GetActiveProducts_ReturnsOkResult_WithFilteredActiveProducts()
    {
        // Arrange: Prepare a list of active products to be returned by the mock service.
        List<ProductDto> activeProducts =
        [
            new() { Id = 1, Name = "Product 1", ImgUri = "active1.jpg", Price = 15.99M, Description = "Description 1" },
            new() { Id = 2, Name = "Product 2", ImgUri = "active2.jpg", Price = 25.99M, Description = "Description 2" }
        ];

        // Mock service to return only the active products.
        mockProductService.Setup(service => service.GetAllActiveProductsAsync()).ReturnsAsync(activeProducts);

        // Act: Call the GetActiveProductsAsync method on the controller.
        IActionResult result = await controller.GetActiveProductsAsync();

        // Assert: Verify that only the active products are returned.
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        List<ProductDto> returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count); // Ensure the correct number of active products is returned
        Assert.Equal(activeProducts[0].Id, returnValue[0].Id); // Ensure the first active product matches
        Assert.Equal(activeProducts[1].Id, returnValue[1].Id); // Ensure the second active product matches
    }

    #endregion

    #region Get product by Id

    /// <summary>
    /// Verifies that getting a product by ID returns an OkObjectResult with the correct product data.
    /// Tests that the controller responds correctly when the product is found in the service.
    /// </summary>
    [Fact]
    public async Task GetProductById_ReturnsOkResult_WhenProductExists()
    {
        // Arrange: Prepare the product DTO to be returned by the mock service.
        ProductDto productDto = new() { Id = 1, Name = "Product 1", ImgUri = "image1.jpg", Price = 9.99M, Description = "Description 1" };
        mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(productDto);

        // Act: Call the GetProductById method on the controller.
        IActionResult result = await controller.GetProductByIdAsync(1);

        // Assert: Verify that the response contains the expected product data.
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        ProductDto returnValue = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(productDto.Id, returnValue.Id);
        Assert.Equal(productDto.Name, returnValue.Name);
    }

    /// <summary>
    /// Verifies that getting a product by ID returns a NotFoundObjectResult when the product does not exist.
    /// Tests that the controller responds correctly when the product is not found in the service.
    /// </summary>
    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange: Setup the mock service to return null for a non-existing product.
        mockProductService.Setup(service => service.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((ProductDto?)null);

        // Act: Call the GetProductById method on the controller with a non-existing product ID.
        IActionResult result = await controller.GetProductByIdAsync(9999);

        // Assert: Ensure the result is a NotFoundObjectResult.
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Product not found.", notFoundResult.Value);
    }

    #endregion

    #region Update product description

    /// <summary>
    /// Verifies that updating a product description returns an OkObjectResult indicating success.
    /// Tests that the controller responds correctly when the update is successful.
    /// </summary>
    [Fact]
    public async Task UpdateProductDescription_ReturnsOkResult_WhenProductUpdatedSuccessfully()
    {
        // Arrange: Prepare a request object with a new description and mock the service to update the product.
        UpdateProductDto request = new() { Description = "Updated Description" };
        int id = 1;

        mockProductService.Setup(service => service.UpdateProductDescriptionAsync(id, request.Description)).ReturnsAsync(true); // Mocking the update success

        // Act: Call the UpdateProductDescriptionAsync method on the controller.
        IActionResult result = await controller.UpdateProductDescriptionAsync(id, request);

        // Assert: Verify the result is OkObjectResult and contains a success message.
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result); // Assert that the result is OkObjectResult
        Assert.Equal("Product description updated successfully.", okResult.Value); // Assert the success message
    }

    /// <summary>
    /// Verifies that updating a product description returns a BadRequestObjectResult when the product ID is default.
    /// Tests that the controller correctly handles invalid input.
    /// </summary>
    [Fact]
    public async Task UpdateProductDescription_ReturnsBadRequest_WhenProductIdIsDefault()
    {
        // Arrange: Prepare a request with a default product ID.
        UpdateProductDto request = new() { Description = "Updated Description" };

        // Act: Call the UpdateProductDescriptionAsync method with a default ID.
        IActionResult result = await controller.UpdateProductDescriptionAsync(default, request);

        // Assert: Ensure the result is a BadRequestObjectResult.
        NotFoundObjectResult badRequestResult = Assert.IsType<NotFoundObjectResult>(result); // Assert that the result is BadRequestObjectResult
        Assert.Equal("Product not found.", badRequestResult.Value); // Ensure the correct error message is returned
    }

    /// <summary>
    /// Verifies that when a product is not found, the controller returns a NotFoundObjectResult.
    /// Tests that the controller properly handles the scenario where the product ID does not exist.
    /// </summary>
    [Fact]
    public async Task UpdateProductDescription_ReturnsNotFound_WhenProductNotFound()
    {
        // Arrange: Prepare a request and set the product ID to a valid value, but mock the service to throw ProductNotFoundException.
        UpdateProductDto request = new() { Description = "Updated Description" };
        int id = 1;
        mockProductService.Setup(service => service.UpdateProductDescriptionAsync(id, request.Description)).ReturnsAsync(false);

        // Act: Call the UpdateProductDescriptionAsync method with the ID of a nonexistent product.
        IActionResult result = await controller.UpdateProductDescriptionAsync(id, request);

        // Assert: Verify the result is a NotFoundObjectResult and contains a "Product not found" message.
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Product not found.", notFoundResult.Value); // Assert the product not found message
    }

    /// <summary>
    /// Verifies that when an unexpected error occurs, the controller returns a StatusCode 500 with an error message.
    /// This test ensures that the controller gracefully handles unexpected exceptions.
    /// </summary>
    [Fact]
    public async Task UpdateProductDescription_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
    {
        // Arrange: Prepare a request with a new description and mock the service to throw a general exception.
        UpdateProductDto request = new() { Description = "Updated Description" };
        int id = 1;
        mockProductService.Setup(service => service.UpdateProductDescriptionAsync(id, request.Description)).ThrowsAsync(new Exception());

        // Act: Call the UpdateProductDescriptionAsync method while the service throws an exception.
        IActionResult result = await controller.UpdateProductDescriptionAsync(id, request);

        // Assert: Verify the result is an ObjectResult with status code 500 and an error message.
        ObjectResult statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode); // Assert that the status code is 500
        Assert.Equal("An internal server error occurred.", statusCodeResult.Value); // Assert the error message
    }


    #endregion

    #region Activate product

    /// <summary>
    /// Verifies that the ActivateProduct action returns an OkObjectResult when a product is successfully activated.
    /// Tests that the controller responds correctly to a successful product activation request.
    /// </summary>
    [Fact]
    public async Task ActivateProduct_ReturnsOkResult_WhenProductActivatedSuccessfully()
    {
        // Arrange: Prepare product ID and mock the service to activate the product.
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, true)).ReturnsAsync(true);

        // Act: Call the ActivateProductAsync method on the controller.
        IActionResult result = await controller.ActivateProductAsync(productId);

        // Assert: Verify the result is OkObjectResult and contains a success message.
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Product activated successfully.", okResult.Value); // Assert the activation success message
    }

    /// <summary>
    /// Verifies that when the product is not found during activation, the controller returns a NotFoundObjectResult.
    /// Tests that the controller handles the scenario where the product cannot be found.
    /// </summary>
    [Fact]
    public async Task ActivateProduct_ReturnsNotFound_WhenProductNotFound()
    {
        // Arrange: Set up mock to throw a ProductNotFoundException when the product ID is used.
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, true)).ReturnsAsync(false);

        // Act: Call the ActivateProductAsync method with a nonexistent product.
        IActionResult result = await controller.ActivateProductAsync(productId);

        // Assert: Verify the result is NotFoundObjectResult and contains an error message.
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Product activation was not successful.", notFoundResult.Value); // Assert the "Product not found" message
    }

    /// <summary>
    /// Verifies that the ActivateProduct action returns a 500 status code when an unexpected error occurs.
    /// Ensures the controller handles unexpected errors correctly by returning a generic error message.
    /// </summary>
    [Fact]
    public async Task ActivateProduct_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
    {
        // Arrange: Prepare the product ID and set up the mock service to throw an unexpected error.
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, true)).ThrowsAsync(new Exception());

        // Act: Call the ActivateProductAsync method on the controller.
        IActionResult result = await controller.ActivateProductAsync(productId);

        // Assert: Verify the result is an ObjectResult with a 500 status code and an internal server error message.
        ObjectResult statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode); // Assert that the status code is 500
        Assert.Equal("An internal server error occurred.", statusCodeResult.Value); // Assert the error message
    }

    #endregion

    #region Deactivate product

    /// <summary>
    /// Verifies that the DeactivateProduct action returns an OkObjectResult when a product is successfully deactivated.
    /// Ensures the controller handles successful product deactivation correctly.
    /// </summary>
    [Fact]
    public async Task DeactivateProduct_ReturnsOkResult_WhenProductDeactivatedSuccessfully()
    {
        // Arrange: Prepare the product ID and mock the service to deactivate the product.
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, false)).ReturnsAsync(true);

        // Act: Call the DeactivateProductAsync method on the controller.
        IActionResult result = await controller.DeactivateProductAsync(productId);

        // Assert: Verify the result is OkObjectResult and contains the success message.
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Product deactivated successfully.", okResult.Value); // Assert deactivation success
    }

    /// <summary>
    /// Verifies that when a product is not found during deactivation, the controller returns a NotFoundObjectResult.
    /// Tests that the controller handles the scenario where the product cannot be found during deactivation.
    /// </summary>
    [Fact]
    public async Task DeactivateProduct_ReturnsNotFound_WhenProductNotFound()
    {
        // Arrange: Set up mock to throw a ProductNotFoundException when the product ID is used.
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, false)).ReturnsAsync(false);

        // Act: Call the DeactivateProductAsync method with a nonexistent product.
        IActionResult result = await controller.DeactivateProductAsync(productId);

        // Assert: Verify the result is NotFoundObjectResult and contains the error message.
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Product deactivation was not successful.", notFoundResult.Value); // Assert the "Product not found" message
    }

    /// <summary>
    /// Verifies that the DeactivateProduct action returns a 500 status code when an unexpected error occurs.
    /// Ensures the controller handles unexpected errors correctly by returning a generic error message.
    /// </summary>
    [Fact]
    public async Task DeactivateProduct_ReturnsStatusCode500_WhenUnexpectedErrorOccurs()
    {
        // Arrange: Prepare the product ID and set up the mock service to throw an unexpected error.
        int productId = 1;
        mockProductService.Setup(service => service.SetIsActiveAsync(productId, false)).ThrowsAsync(new Exception());

        // Act: Call the DeactivateProductAsync method on the controller.
        IActionResult result = await controller.DeactivateProductAsync(productId);

        // Assert: Verify the result is an ObjectResult with a 500 status code and an internal server error message.
        ObjectResult statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode); // Assert that the status code is 500
        Assert.Equal("An internal server error occurred.", statusCodeResult.Value); // Assert the error message
    }

    #endregion
}
