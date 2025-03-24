using System.ComponentModel.DataAnnotations;

namespace Domain.Models.ProductManagement;

/// <summary>
/// Represents a category of products in the system.
/// </summary>
public class ProductCategory : DbEntity
{
    /// <summary>
    /// Gets or sets the name of the product category.
    /// </summary>
    [MaxLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the product category.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the collection of products associated with the product category.
    /// </summary>
    public ICollection<Product> Products { get; set; } = [];
}