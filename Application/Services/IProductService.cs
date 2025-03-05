using Application.DTOs;

namespace Application.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<List<ProductDto>> GetProductsAsync(int page, int pageSize);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<bool> UpdateProductDescriptionAsync(UpdateProductDto productDto);
    }
}
