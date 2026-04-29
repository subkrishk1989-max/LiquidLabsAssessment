using LiquidLabsAssessment.Domain.Entities;

namespace LiquidLabsAssessment.Application.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task InsertAsync(Product product);
}
