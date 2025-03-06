using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2); // Precision: 18 digits, Scale: 2 digits after the decimal
        modelBuilder.Entity<Product>()
               .Property(p => p.IsActive)
               .HasDefaultValue(true); // Set the default value for IsActive
    }
}
