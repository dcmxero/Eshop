using Domain;

namespace Infrastructure.Repositories;

public class GenericRepository<TEntity>(ApplicationDbContext context) where TEntity : DbEntity
{
    protected readonly ApplicationDbContext context = context;

    public IQueryable<TEntity> GetAll()
    {
        return context.Set<TEntity>().AsQueryable();
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Set<TEntity>().FindAsync([id], cancellationToken);
    }
}
