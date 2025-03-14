using Swashbuckle.AspNetCore.Annotations;

namespace Application.DTOs;

/// <summary>
/// Represents the data transfer object (DTO) for updating a product.
/// </summary>
public class UpdateProductDto
{
    /// <summary>
    /// Gets or sets the updated description of the product.
    /// </summary>
    [SwaggerSchema(Description = "The updated description of the product.")]
    public required string Description { get; set; }
}