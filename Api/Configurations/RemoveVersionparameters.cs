using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Configurations;

public class RemoveVersionParameters : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Only remove version parameters from the documentation, not from the actual API calls
        if (context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<ApiVersionAttribute>().Any())
        {
            // Remove version parameters from query, header, or path if it's present in documentation
            List<OpenApiParameter> versionParameters = [.. operation.Parameters.Where(p => p.Name == "version"
                                                                   || p.Name == "api-version"
                                                                   || p.Name == "x-api-version")];

            foreach (OpenApiParameter? versionParam in versionParameters)
            {
                operation.Parameters.Remove(versionParam);
            }
        }
    }
}