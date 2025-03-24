using Application.Features.Products;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories.ProductManagement;
using Infrastructure.Seeds;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApi.Configurations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure the DbContext for SQL Server using the connection string from the configuration.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Unit of Work for dependency injection.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Product repository and service for dependency injection.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register MediatR services, pointing to the assembly where handlers are located.
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetProductsHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetActiveProductsHandler).Assembly));

// Register controller services to handle incoming HTTP requests.
builder.Services.AddControllers();

// Register Swagger/OpenAPI services for API documentation.
builder.Services.AddEndpointsApiExplorer();

// API Versioning configuration.
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Reports available versions in response headers.
    options.AssumeDefaultVersionWhenUnspecified = true; // Uses default version if not specified.
    options.DefaultApiVersion = new ApiVersion(1, 0); // Sets default API version.
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Disables query string and header-based versioning.
});

// API Explorer configuration to expose versioned endpoints.
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Defines format for versioning (e.g., v1, v2).
    options.SubstituteApiVersionInUrl = true; // Substitutes API version in URL for versioned endpoints.
});

builder.Services.AddSwaggerGen(options =>
{
    // Defines Swagger documentation for version 1 of the API.
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API for managing products"
    });

    // Defines Swagger documentation for version 2 of the API.
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "My API",
        Version = "v2",
        Description = "API for managing products allowing getting products with pagination"
    });

    options.EnableAnnotations(); // Enables annotations for better API documentation.
    options.DocumentFilter<RemoveVersionParameters>(); // Removes version parameters from API documentation.
});

// Clears existing logging providers and adds console logging.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

WebApplication app = builder.Build();

// Apply database migrations automatically when the application starts.
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate(); // Applies any pending migrations automatically.
}

// Check if the environment is Development.
if (app.Environment.IsDevelopment())
{
    DataSeed.Initialize(app.Services); // Initializes product seed data in the development environment.

    app.UseSwagger(); // Enables Swagger for API documentation.
    app.UseSwaggerUI(options =>
    {
        IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant()); // Adds Swagger endpoints for each API version.
        }
    });
}

// Configures middleware to handle HTTPS redirection and authorization.
app.UseHttpsRedirection();
app.UseAuthorization();

// Maps API controllers to endpoints.
app.MapControllers();

// Starts the application.
app.Run();
