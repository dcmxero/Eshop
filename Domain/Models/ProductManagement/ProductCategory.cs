namespace Domain.Models.ProductManagement;

/// <summary>
/// Represents a category of products in the system.
/// </summary>
public class ProductCategory : DbEntity
{
    /// <summary>
    /// Gets or sets the name of the product category.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the product category.
    /// </summary>
    public string? Description { get; set; }
}