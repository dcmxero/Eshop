using Domain.Models;
using Infrastructure.Exceptions;
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

    public async Task<List<Product>> GetProductsAsync(int page, int pageSize)
    {
        return await context
            .Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        Product? product = await context.Products.FindAsync(id);
        return product ?? throw new ProductNotFoundException($"Product with ID {id} was not found.");
    }

    public async Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }
}
