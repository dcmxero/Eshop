using Application.DTOs;
using Application.Services;
using Infrastructure.Repositories;
using Xunit;

namespace Application.Tests;

public class ProductServiceTests
{
    private readonly IProductRepository productRepository;

    public ProductServiceTests()
    {
        productRepository = new MockProductRepository();
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnCorrectNumberOfProducts()
    {
        // Arrange
        ProductService productService = new(productRepository);

        // Act
        List<ProductDto> products = await productService.GetAllProductsAsync(1, 10);

        // Assert
        Assert.Equal(5, products.Count);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnCorrectProduct()
    {
        // Arrange
        ProductService productService = new(productRepository);

        // Act
        ProductDto product = await productService.GetProductByIdAsync(1);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
        Assert.Equal("Product 1", product.Name);
    }
}
