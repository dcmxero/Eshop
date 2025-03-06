using Application.DTOs;
using Domain.Models;

namespace Application.Mappers;

public static class ProductMapper
{
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
