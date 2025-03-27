namespace Infrastructure.UnitOfWork;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext context = context;

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
