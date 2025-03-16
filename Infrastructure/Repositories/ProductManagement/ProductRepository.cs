using Domain.Models.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProductManagement;

public class ProductRepository(ApplicationDbContext context) : GenericRepository<Product>(context), IProductRepository
{
    public new async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        // Eagerly load the ProductCategory when retrieving the Product
        return await context.Products
            .Where(p => p.Id == productId)
            .Include(p => p.ProductCategory) // Eagerly load ProductCategory
            .FirstOrDefaultAsync(cancellationToken); // Fetch the product with the category
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