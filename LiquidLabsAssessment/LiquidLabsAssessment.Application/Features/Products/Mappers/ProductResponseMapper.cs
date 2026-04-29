using LiquidLabsAssessment.Application.Features.Products.DTOs;
using LiquidLabsAssessment.Domain.Entities;

namespace LiquidLabsAssessment.Application.Features.Products.Mappers;

public static class ProductResponseMapper
{
    public static ProductResponseDto Map(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,

            Data = product.Attributes.ToDictionary(
                x => x.AttributeName,
                x => (object?)x.AttributeValue)
        };
    }
}
