using Application.Features.Products;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Seeds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure the DbContext for SQL Server using the connection string from the configuration.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Product repository and service for dependency injection.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register MediatR services, pointing to the assembly where your handlers are located
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetProductsHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetActiveProductsHandler).Assembly));


// Register the controller services to handle incoming HTTP requests.
builder.Services.AddControllers();

// API Versioning configuration
builder.Services.AddApiVersioning(options =>
{
    // Assume default version (1.0) when the version is not specified in the request.
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Set default API version to 1.0.
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Configure API version reading from request header or query string.
    options.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("api-version")
    );

    // Enable reporting of available API versions.
    options.ReportApiVersions = true;
});

// API Explorer configuration to expose versioned endpoints.
builder.Services.AddVersionedApiExplorer(options =>
{
    // Define format for versioning (e.g., v1, v2).
    options.GroupNameFormat = "'v'VVV";

    // Substitute API version in URL for versioned endpoints.
    options.SubstituteApiVersionInUrl = true;
});

// Register Swagger/OpenAPI services for API documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Define Swagger documentation for version 1 of the API.
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API for managing products"
    });

    // Define Swagger documentation for version 2 of the API.
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "My API",
        Version = "v2",
        Description = "API for managing products allowing getting products with pagination"
    });

    // Enable annotations for better API documentation.
    options.EnableAnnotations();
});

// Clear existing logging providers and add console logging.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

WebApplication app = builder.Build();

// Check if the environment is Development.
if (app.Environment.IsDevelopment())
{
    // Initialize product seed data in development environment.
    ProductSeed.Initialize(app.Services);

    // Enable Swagger and Swagger UI for API documentation.
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Get the API version descriptions for Swagger UI.
        IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
        {
            // Add Swagger endpoints for each API version.
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

// Apply database migrations automatically when the application starts.
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

    // Apply any pending migrations automatically.
    context.Database.Migrate();
}

// Configure middleware to handle HTTPS redirection and authorization.
app.UseHttpsRedirection();
app.UseAuthorization();

// Map API controllers to endpoints.
app.MapControllers();

// Start the application.
app.Run();
