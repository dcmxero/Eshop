using Domain.Models.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Represents the database context for managing products.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with the specified options.
/// </remarks>
/// <param name="options">The options to configure the context.</param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the collection of products in the database.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets or sets the collection of product categories in the database.
    /// </summary>
    public DbSet<ProductCategory> ProductCategories { get; set; }

    /// <summary>
    /// Configures the model for the <see cref="ApplicationDbContext"/>.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the model.</param>
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