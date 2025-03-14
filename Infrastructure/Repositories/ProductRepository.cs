using Domain.Models;
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

    public async Task<List<Product>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context
            .Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<Product>> GetActiveProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context
            .Products
            .Where(x => x.IsActive)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);
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