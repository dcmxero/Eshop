using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi
{
    public class RemoveVersionParameters : IOperationFilter
    {
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
}
