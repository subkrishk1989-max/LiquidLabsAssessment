using LiquidLabsAssessment.Domain.Entities;
using LiquidLabsAssessment.Infrastructure.External.DTOs;

namespace LiquidLabsAssessment.Infrastructure.Mappers;

public static class ProductMapper
{
    public static Product Map(ProductApiResponse apiProduct)
    {
        var product = new Product
        {
            Id = int.TryParse(apiProduct.Id, out var id) ? id : 0,
            Name = apiProduct.Name
        };

        if (apiProduct.Data != null)
        {
            foreach (var property in apiProduct.Data.Properties())
            {
                product.Attributes.Add(new ProductAttribute
                {
                    ProductId = Convert.ToInt32(apiProduct.Id),
                    AttributeName = property.Name,
                    AttributeValue = property.Value?.ToString()
                });
            }
        }
        return product;
    }
}
