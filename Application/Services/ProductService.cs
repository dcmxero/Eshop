using Application.DTOs;
using Application.Mappers;
using Domain.Models;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository productRepository = productRepository;

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            List<Product> products = await productRepository.GetAllProductsAsync();
            return [.. products.Select(ProductMapper.ToDto)];
        }

        public async Task<List<ProductDto>> GetProductsAsync(int page, int pageSize)
        {
            List<Product> products = await productRepository.GetProductsAsync(page, pageSize);
            return [.. products.Select(ProductMapper.ToDto)];
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            Product product = await productRepository.GetByIdAsync(id);
            return product.ToDto();
        }

        public async Task<bool> UpdateProductDescriptionAsync(UpdateProductDto productDto)
        {
            Product product = await productRepository.GetByIdAsync(productDto.Id);
            if (product == null)
            {
                return false;
            }

            product.Description = productDto.Description;

            await productRepository.UpdateAsync(product);
            return true;
        }
    }
}