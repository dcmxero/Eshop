using Swashbuckle.AspNetCore.Annotations;

namespace DTOs.Product;

/// <summary>
/// Represents a product data transfer object (DTO).
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    [SwaggerSchema(Description = "The unique identifier of the product.")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    [SwaggerSchema(Description = "The name of the product.")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the image URI of the product.
    /// </summary>
    [SwaggerSchema(Description = "The URI of the product image.")]
    public required string ImgUri { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    [SwaggerSchema(Description = "The price of the product.")]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    [SwaggerSchema(Description = "The detailed description of the product.")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is active.
    /// </summary>
    [SwaggerSchema(Description = "Indicates if the product is active.")]
    public bool IsActive { get; set; }
}
