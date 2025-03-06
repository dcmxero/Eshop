using Application.DTOs;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Controllers.v2;
using Xunit;

namespace WebApi.Tests;

/// <summary>
/// Unit tests for the ProductsControllerV2 class in the WebApi.Tests namespace.
/// Verifies functionality for retrieving products and active products from the ProductsController.
/// </summary>
public class ProductsControllerV2Tests
{
    private readonly IProductService productService;
    private readonly ProductsController controller;

    /// <summary>
    /// Initializes a new instance of the ProductsControllerV2Tests class.
    /// Creates an instance of the ProductsController using either a mock or actual product service based on configuration.
    /// </summary>
    public ProductsControllerV2Tests()
    {
        productService = CreateProductService();
        controller = new ProductsController(productService);
    }

    /// <summary>
    /// Creates an instance of the IProductService based on the configuration from appsettings.json.
    /// If UseMockProductService is true, a mock product service is used; otherwise, a real product service is created using the database.
    /// </summary>
    /// <returns>The product service to be used in the controller.</returns>
    private static IProductService CreateProductService()
    {
        // Load configuration from appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Get the value of UseMockProductService from appsettings.json
        bool useMock = configuration.GetValue<bool>("UseMockProductService");

        if (useMock)
        {
            return new MockProductService(); // Use mock data
        }
        else
        {
            // Get the connection string from appsettings.json
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("DefaultConnection not found");
            }

            // Configure the DbContext with the connection string
            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(connectionString); // You can replace UseSqlServer if using another database provider

            ApplicationDbContext dbContext = new(optionsBuilder.Options);

            // Create the repository using the dbContext
            IProductRepository productRepository = new ProductRepository(dbContext);

            // Return the actual ProductService that uses the repository
            return new ProductService(productRepository);
        }
    }

    /// <summary>
    /// Tests that the correct number of products is returned when page size is specified.
    /// Verifies that only the expected number of products are included in the response and that the last product matches the expected name.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetProducts_ReturnsCorrectNumberOfProducts_WhenPageSizeIsSpecified()
    {
        // Arrange
        int page = 1;
        int pageSize = 5;

        // Act
        IActionResult result = await controller.GetProductsAsync(page, pageSize);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        List<ProductDto> returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.Equal(pageSize, returnValue.Count); // Verify that only the correct number of products are returned
        Assert.Equal("Product 5", returnValue[^1].Name); // Ensure the last product returned matches
    }

    /// <summary>
    /// Tests that only active products are returned when the page size is specified.
    /// Verifies that only active products are included in the response and that the correct number of active products is returned.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of products per page.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetActiveProducts_ReturnsOnlyActiveProducts_WhenPageSizeIsSpecified()
    {
        // Arrange
        int page = 1;
        int pageSize = 5;

        // Act
        IActionResult result = await controller.GetActiveProductsAsync(page, pageSize);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        List<ProductDto> returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);

        // Ensure only active products are returned
        Assert.All(returnValue, product => Assert.True(product.IsActive));

        // Ensure the correct number of active products are returned based on the page size
        Assert.Equal(4, returnValue.Count); // Only 4 active products are available
    }
}
