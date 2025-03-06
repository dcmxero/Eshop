using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

/// <summary>
/// Factory class to create an instance of <see cref="ApplicationDbContext"/> for design-time operations.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="ApplicationDbContext"/> with the appropriate configuration for design-time operations.
    /// </summary>
    /// <param name="args">Arguments passed during context creation (not used in this case).</param>
    /// <returns>A new instance of <see cref="ApplicationDbContext"/>.</returns>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Set up configuration to read the connection string
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        // Set up the DbContext to use SQL Server
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}