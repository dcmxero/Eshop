using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Configurations;

/// <summary>
/// An operation filter that removes version parameters from the operation.
/// This includes query parameters, headers, and path parameters related to API versioning.
/// </summary>
public class RemoveVersionParameters : IOperationFilter
{
    /// <summary>
    /// Applies the operation filter to remove version-related parameters (e.g., version, api-version, x-api-version).
    /// </summary>
    /// <param name="operation">The OpenApi operation to modify.</param>
    /// <param name="context">The context for the operation, containing information about the operation and the API.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Remove the version parameter from query, header, or path if it's present
        OpenApiParameter? versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "version");
        if (versionParameter != null)
        {
            operation.Parameters.Remove(versionParameter);
        }

        OpenApiParameter? apiVersionQueryParameter = operation.Parameters.FirstOrDefault(p => p.Name == "api-version");
        if (apiVersionQueryParameter != null)
        {
            operation.Parameters.Remove(apiVersionQueryParameter);
        }

        OpenApiParameter? apiVersionHeaderParameter = operation.Parameters.FirstOrDefault(p => p.Name == "x-api-version");
        if (apiVersionHeaderParameter != null)
        {
            operation.Parameters.Remove(apiVersionHeaderParameter);
        }
    }
}
