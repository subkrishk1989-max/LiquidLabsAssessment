namespace LiquidLabsAssessment.Application.Features.Products.DTOs;

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public Dictionary<string, object?> Data { get; set; } = new();
}
