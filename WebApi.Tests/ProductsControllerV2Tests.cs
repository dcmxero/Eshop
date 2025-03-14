using Application.Features.Products;
using Application.Services;
using DTOs.Common;
using DTOs.Product;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Controllers.v2;
using Xunit;

namespace WebApi.Tests;

/// <summary>
/// Unit tests for the ProductsControllerV2 class in the WebApi.Tests namespace.
/// Verifies functionality for retrieving products and active products from the ProductsController.
/// </summary>
public class ProductsControllerV2Tests
{
    private readonly IMediator mediator;
    private readonly ProductsController controller;

    /// <summary>
    /// Initializes a new instance of the ProductsControllerV2Tests class.
    /// Creates an instance of the ProductsController using either a mock or actual product service based on configuration.
    /// </summary>
    public ProductsControllerV2Tests()
    {
        mediator = CreateMediator();
        controller = new ProductsController(mediator);
    }

    /// <summary>
    /// Creates an instance of IMediator based on the configuration from appsettings.json.
    /// If UseMockMediator is true, a mock mediator with predefined responses is used; otherwise, the real mediator is created for actual data handling.
    /// </summary>
    /// <returns>The IMediator to be used in the application or tests.</returns>
    private static IMediator CreateMediator()
    {
        // Load configuration from appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Get the value of UseMockData from appsettings.json
        bool useMock = configuration.GetValue<bool>("UseMockData");

        if (useMock)
        {
            Mock<IMediator> mediatorMock = new();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetProductsRequest request, CancellationToken token) =>
                {
                    List<ProductDto> allProducts =
                    [
                        new() { Id = 1, Name = "Product 1", ImgUri = "product1.jpg", Price = 10.99M, IsActive = true },
                        new() { Id = 2, Name = "Product 2", ImgUri = "product2.jpg", Price = 12.99M, IsActive = false },
                        new() { Id = 3, Name = "Product 3", ImgUri = "product3.jpg", Price = 14.99M, IsActive = true },
                        new() { Id = 4, Name = "Product 4", ImgUri = "product4.jpg", Price = 16.99M, IsActive = true },
                        new() { Id = 5, Name = "Product 5", ImgUri = "product5.jpg", Price = 18.99M, IsActive = false },
                        new() { Id = 6, Name = "Product 6", ImgUri = "product6.jpg", Price = 20.99M, IsActive = true }
                    ];

                    // Paginate the result
                    List<ProductDto> paginatedProducts = [.. allProducts
                        .Skip((request.Page - 1) * request.PageSize)
                        .Take(request.PageSize)];

                    return new PaginatedList<ProductDto>(paginatedProducts, allProducts.Count, request.Page, request.PageSize);
                });

            // Setup mock response for GetActiveProductsRequest
            mediatorMock.Setup(m => m.Send(It.IsAny<GetActiveProductsRequest>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((GetActiveProductsRequest request, CancellationToken token) =>
                 {
                     List<ProductDto> allProducts =
                     [
                         new() { Id = 1, Name = "Product 1", ImgUri = "product1.jpg", Price = 10.99M, IsActive = true },
                        new() { Id = 2, Name = "Product 2", ImgUri = "product2.jpg", Price = 12.99M, IsActive = false },
                        new() { Id = 3, Name = "Product 3", ImgUri = "product3.jpg", Price = 14.99M, IsActive = true },
                        new() { Id = 4, Name = "Product 4", ImgUri = "product4.jpg", Price = 16.99M, IsActive = true },
                        new() { Id = 5, Name = "Product 5", ImgUri = "product5.jpg", Price = 18.99M, IsActive = false },
                        new() { Id = 6, Name = "Product 6", ImgUri = "product6.jpg", Price = 20.99M, IsActive = true }
                     ];

                     // Paginate the result
                     List<ProductDto> paginatedProducts = [.. allProducts
                        .Where(x => x.IsActive)
                        .Skip((request.Page - 1) * request.PageSize)
                        .Take(request.PageSize)];

                     return new PaginatedList<ProductDto>(paginatedProducts, allProducts.Count, request.Page, request.PageSize);
                 });

            return mediatorMock.Object; // Return the mocked mediator
        }
        else
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))) // Register your DbContext
                .AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetProductsHandler).Assembly)) // Register MediatR
                .AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetActiveProductsHandler).Assembly)) // Register MediatR
                .AddScoped<IProductService, ProductService>()    // Register ProductService
                .AddScoped<IProductRepository, ProductRepository>() // Register ProductRepository
                .BuildServiceProvider();

            return serviceProvider.GetRequiredService<IMediator>(); // Return the real mediator
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
        IActionResult result = await controller.GetProductsV2Async(page, pageSize);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        PaginatedList<ProductDto> response = Assert.IsType<PaginatedList<ProductDto>>(okResult.Value);
        Assert.Equal(pageSize, response.Data.Count); // Verify that only the correct number of products are returned
        Assert.Equal("Product 5", response.Data[^1].Name); // Ensure the last product returned matches
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
        IActionResult result = await controller.GetActiveProductsV2Async(page, pageSize);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        PaginatedList<ProductDto> returnValue = Assert.IsType<PaginatedList<ProductDto>>(okResult.Value);

        // Ensure only active products are returned
        Assert.All(returnValue.Data, product => Assert.True(product.IsActive));

        // Ensure the correct number of active products are returned based on the page size
        Assert.Equal(4, returnValue.Data.Count); // Only 4 active products are available
    }
}