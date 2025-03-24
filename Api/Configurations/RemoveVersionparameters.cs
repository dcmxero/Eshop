using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Configurations;

public class RemoveVersionParameters : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Loop through the paths and remove any version parameter from the paths
        foreach (KeyValuePair<string, OpenApiPathItem> path in swaggerDoc.Paths)
        {
            IDictionary<OperationType, OpenApiOperation> operations = path.Value.Operations;

            foreach (KeyValuePair<OperationType, OpenApiOperation> operation in operations)
            {
                List<OpenApiParameter> parametersToRemove = [];

                foreach (OpenApiParameter? parameter in operation.Value.Parameters)
                {
                    // Remove the api-version and x-api-version parameters
                    if (parameter.Name == "api-version" || parameter.Name == "x-api-version")
                    {
                        parametersToRemove.Add(parameter);
                    }
                }

                // Remove them after iterating
                foreach (OpenApiParameter parameter in parametersToRemove)
                {
                    operation.Value.Parameters.Remove(parameter);
                }
            }
        }
    }
}