using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    private readonly ApplicationDbContext context = context;

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await context
            .Products
            .ToListAsync();
    }

    public async Task<List<Product>> GetAllActiveProductsAsync()
    {
        return await context
            .Products
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsAsync(int page, int pageSize)
    {
        return await context
            .Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Product>> GetActiveProductsAsync(int page, int pageSize)
    {
        return await context
            .Products
            .Where(x => x.IsActive)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int productId)
    {
        return await context.Products.FirstOrDefaultAsync(x => x.Id == productId);
    }

    public async Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task<bool> SetIsActiveAsync(int productId, bool isActive)
    {
        Product? product = await context.Products.FirstOrDefaultAsync(x => x.Id == productId);
        if (product == null)
        {
            return false;
        }
        product.IsActive = isActive;
        await context.SaveChangesAsync();
        return true;
    }
}
