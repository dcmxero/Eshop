using Application.DTOs;
using Application.Mappers;
using Domain.Models;
using Infrastructure.Exceptions;
using Infrastructure.Repositories;

namespace Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository productRepository = productRepository;

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        List<Product> products = await productRepository.GetAllProductsAsync();
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task<List<ProductDto>> GetAllActiveProductsAsync()
    {
        List<Product> products = await productRepository.GetAllActiveProductsAsync();
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task<List<ProductDto>> GetProductsAsync(int page, int pageSize)
    {
        List<Product> products = await productRepository.GetProductsAsync(page, pageSize);
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task<List<ProductDto>> GetActiveProductsAsync(int page, int pageSize)
    {
        List<Product> products = await productRepository.GetActiveProductsAsync(page, pageSize);
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task UpdateProductDescriptionAsync(int productId, string? description)
    {
        Product product = await productRepository.GetByIdAsync(productId) ?? throw new ProductNotFoundException();

        product.Description = description;

        await productRepository.UpdateAsync(product);
    }

    public async Task SetIsActiveAsync(int productId, bool isActive)
    {
        await productRepository.SetIsActiveAsync(productId, isActive);
    }
}