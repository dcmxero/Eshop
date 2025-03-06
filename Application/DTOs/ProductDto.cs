namespace Application.DTOs;

/// <summary>
/// Represents a product data transfer object (DTO).
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the image URI of the product.
    /// </summary>
    public required string ImgUri { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is active.
    /// </summary>
    public bool IsActive { get; set; }
}
