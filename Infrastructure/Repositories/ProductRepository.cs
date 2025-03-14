using Domain.Models;
using DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    private readonly ApplicationDbContext context = context;

    public async Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        return await context
            .Products
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<Product>> GetAllActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        return await context
            .Products
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<DataResultDto<Product>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        // Get the paginated products from the database
        List<Product> products = await context
            .Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Get the total count of products
        int totalCount = await context.Products.CountAsync(cancellationToken);

        return new DataResultDto<Product>
        {
            Data = products, // List of products for the current page
            Count = totalCount // Total count of products
        };
    }

    public async Task<DataResultDto<Product>> GetActiveProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        // Get the paginated active products from the database
        List<Product> products = await context
            .Products
            .Where(x => x.IsActive)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Get the total count of active products
        int totalCount = await context.Products.CountAsync(x => x.IsActive, cancellationToken);

        return new DataResultDto<Product>
        {
            Data = products, // List of active products for the current page
            Count = totalCount // Total count of active products
        };
    }

    public async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await context.Products.FirstOrDefaultAsync(x => x.Id == productId, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SetIsActiveAsync(int productId, bool isActive, CancellationToken cancellationToken = default)
    {
        Product? product = await context.Products.FirstOrDefaultAsync(x => x.Id == productId, cancellationToken: cancellationToken);
        if (product == null)
        {
            return false;
        }
        product.IsActive = isActive;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}