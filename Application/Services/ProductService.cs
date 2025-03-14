using Application.DTOs;
using Application.Mappers;
using Domain.Models;
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

    public async Task<ProductDto?> GetProductByIdAsync(int productId)
    {
        Product? product = await productRepository.GetByIdAsync(productId);
        ProductDto? productDto = product?.ToDto();

        return productDto;
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

    public async Task<bool> UpdateProductDescriptionAsync(int productId, string? description)
    {
        try
        {
            Product? product = await productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return false;
            }

            product.Description = description;

            await productRepository.UpdateAsync(product);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the product.", ex);
        }
    }

    public async Task<bool> SetIsActiveAsync(int productId, bool isActive)
    {
        return await productRepository.SetIsActiveAsync(productId, isActive);
    }
}