using Application.Mappers;
using Domain.Models.ProductManagement;
using DTOs.Common;
using DTOs.Product;
using Infrastructure.Repositories.ProductManagement;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ProductService(IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IProductService
{
    private readonly IProductRepository productRepository = productRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<List<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        List<Product> products = await productRepository
            .GetAll()
            .Include(p => p.ProductCategory)
            .ToListAsync(cancellationToken);
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task<List<ProductDto>> GetAllActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        List<Product> products = await productRepository
            .GetAll()
            .Where(x => x.IsActive)
            .Include(p => p.ProductCategory)
            .ToListAsync(cancellationToken);
        return [.. products.Select(ProductMapper.ToDto)];
    }

    public async Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        Product? product = await productRepository.GetByIdAsync(productId, cancellationToken);
        ProductDto? productDto = product?.ToDto();

        return productDto;
    }

    public async Task<PaginatedList<ProductDto>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        // Get the paginated products from the database
        List<Product> products = await productRepository
            .GetAll()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.ProductCategory)
            .ToListAsync(cancellationToken);

        // Get the total count of products
        int totalCount = await productRepository
            .GetAll()
            .Include(p => p.ProductCategory)
            .CountAsync(cancellationToken);

        return new PaginatedList<ProductDto>([.. products.Select(ProductMapper.ToDto)], totalCount, page, pageSize);
    }

    public async Task<PaginatedList<ProductDto>> GetActiveProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        // Get the paginated products from the database
        List<Product> products = await productRepository
            .GetAll()
            .Where(x => x.IsActive)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.ProductCategory)
            .ToListAsync(cancellationToken);

        // Get the total count of products
        int totalCount = await productRepository
            .GetAll()
            .Where(x => x.IsActive)
            .Include(p => p.ProductCategory)
            .CountAsync(cancellationToken);

        return new PaginatedList<ProductDto>([.. products.Select(ProductMapper.ToDto)], totalCount, page, pageSize);
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

            await unitOfWork.CompleteAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the product description.", ex);
        }
    }

    public async Task<bool> SetIsActiveAsync(int productId, bool isActive, CancellationToken cancellationToken = default)
    {
        try
        {
            Product? product = await productRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                return false;
            }

            product.IsActive = isActive;

            await unitOfWork.CompleteAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the product's active status.", ex);
        }
    }
}