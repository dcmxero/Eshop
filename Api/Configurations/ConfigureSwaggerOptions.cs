using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Configurations;

/// <summary>
/// Configures Swagger options for the API, including versioning and API descriptions.
/// </summary>
public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The API version description provider.</param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Configures the Swagger options with API versions, descriptions, and annotations.
    /// </summary>
    /// <param name="options">The Swagger generation options to configure.</param>
    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "My API",
            Version = "v1",
            Description = "API for managing products"
        });

        options.SwaggerDoc("v2", new OpenApiInfo
        {
            Title = "My API",
            Version = "v2",
            Description = "API for managing products allowing getting products with pagination"
        });

        options.EnableAnnotations();
    }

    /// <summary>
    /// Configures Swagger options for a specific name.
    /// </summary>
    /// <param name="name">The name of the Swagger options configuration.</param>
    /// <param name="options">The Swagger generation options to configure.</param>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}
