using Application.DTOs;
using Application.Mappers;
using Domain.Models;
using Infrastructure.Repositories;

namespace Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository productRepository = productRepository;

    public async Task<List<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        List<Product> products = await productRepository.GetAllProductsAsync(cancellationToken);
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task<List<ProductDto>> GetAllActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        List<Product> products = await productRepository.GetAllActiveProductsAsync(cancellationToken);
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        Product? product = await productRepository.GetByIdAsync(productId, cancellationToken);
        ProductDto? productDto = product?.ToDto();

        return productDto;
    }

    public async Task<List<ProductDto>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        List<Product> products = await productRepository.GetProductsAsync(page, pageSize, cancellationToken);
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task<List<ProductDto>> GetActiveProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        List<Product> products = await productRepository.GetActiveProductsAsync(page, pageSize, cancellationToken);
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task<bool> UpdateProductDescriptionAsync(int productId, string? description, CancellationToken cancellationToken = default)
    {
        try
        {
            Product? product = await productRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                return false;
            }

            product.Description = description;

            await productRepository.UpdateAsync(product, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the product.", ex);
        }
    }

    public async Task<bool> SetIsActiveAsync(int productId, bool isActive, CancellationToken cancellationToken = default)
    {
        return await productRepository.SetIsActiveAsync(productId, isActive, cancellationToken);
    }
}