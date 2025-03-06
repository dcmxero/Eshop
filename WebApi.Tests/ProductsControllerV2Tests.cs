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

public class ProductsControllerV2Tests
{
    private readonly IProductService productService;
    private readonly ProductsController controller;

    public ProductsControllerV2Tests()
    {
        productService = CreateProductService();
        controller = new ProductsController(productService);
    }

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

    [Fact]
    public async Task GetProducts_ReturnsCorrectNumberOfProducts_WhenPageSizeIsSpecified()
    {
        // Arrange
        int page = 1;
        int pageSize = 5;

        // Act
        IActionResult result = await controller.GetProducts(page, pageSize);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        List<ProductDto> returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.Equal(pageSize, returnValue.Count); // Verify that only the correct number of products are returned
        Assert.Equal("Product 5", returnValue[^1].Name); // Ensure the last product returned matches
    }


    [Fact]
    public async Task GetActiveProducts_ReturnsOnlyActiveProducts_WhenPageSizeIsSpecified()
    {
        // Arrange
        int page = 1;
        int pageSize = 5;

        // Act
        IActionResult result = await controller.GetActiveProducts(page, pageSize);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        List<ProductDto> returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);

        // Ensure only active products are returned
        Assert.All(returnValue, product => Assert.True(product.IsActive));

        // Ensure the correct number of active products are returned based on the page size
        Assert.Equal(4, returnValue.Count); // Only 4 active products are available
    }
}
