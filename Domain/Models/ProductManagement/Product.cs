namespace Domain.Models.ProductManagement;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class Product : DbEntity
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the URI of the product image.
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
    /// Gets or sets the active status of the product. Defaults to true.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the ID of the product category.
    /// </summary>
    public int ProductCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the associated product category.
    /// </summary>
    public virtual ProductCategory? ProductCategory { get; set; }
}