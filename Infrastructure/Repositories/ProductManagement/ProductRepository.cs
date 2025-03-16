using Domain.Models.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProductManagement;

public class ProductRepository(ApplicationDbContext context) : GenericRepository<Product>(context), IProductRepository
{
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