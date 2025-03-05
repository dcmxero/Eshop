using Infrastructure;
using Infrastructure.Seeds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

// Register Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eshop API", Version = "v1" });
});

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Optional, to report available API versions in responses
    options.AssumeDefaultVersionWhenUnspecified = true; // Optional, to assume a default version if not specified
    options.DefaultApiVersion = ApiVersion.Default; // Default version if none is specified
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eshop API v1");
    });
}

if (app.Environment.IsDevelopment())
{
    ProductSeed.Initialize(app.Services);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
