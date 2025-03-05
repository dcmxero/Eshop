﻿using Domain.Models;

namespace Infrastructure.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
    Task<List<Product>> GetProductsAsync(int page, int pageSize);
    Task<Product> GetByIdAsync(int id);
    Task UpdateAsync(Product product);
}
