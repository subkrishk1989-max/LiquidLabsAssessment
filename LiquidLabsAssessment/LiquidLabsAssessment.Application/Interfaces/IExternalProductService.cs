using LiquidLabsAssessment.Domain.Entities;

namespace LiquidLabsAssessment.Application.Interfaces;

public interface IExternalProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(string id);
}
