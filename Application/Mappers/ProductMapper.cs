using Application.DTOs;
using Domain.Models;

namespace Application.Mappers;

/// <summary>
/// Provides mapping methods between <see cref="Product"/> and <see cref="ProductDto"/>.
/// </summary>
public static class ProductMapper
{
    /// <summary>
    /// Converts a <see cref="Product"/> domain model to a <see cref="ProductDto"/> data transfer object (DTO).
    /// </summary>
    /// <param name="product">The product domain model to be converted.</param>
    /// <returns>A <see cref="ProductDto"/> containing the data from the provided <see cref="Product"/>.</returns>
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            ImgUri = product.ImgUri,
            Price = product.Price,
            Description = product.Description,
            IsActive = product.IsActive
        };
    }

    /// <summary>
    /// Converts a <see cref="ProductDto"/> data transfer object (DTO) to a <see cref="Product"/> domain model.
    /// </summary>
    /// <param name="productDto">The product DTO to be converted.</param>
    /// <returns>A <see cref="Product"/> domain model containing the data from the provided <see cref="ProductDto"/>.</returns>
    public static Product ToDomain(this ProductDto productDto)
    {
        return new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            ImgUri = productDto.ImgUri,
            Price = productDto.Price,
            Description = productDto.Description,
            IsActive = productDto.IsActive
        };
    }
}
