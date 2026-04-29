namespace LiquidLabsAssessment.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<ProductAttribute> Attributes { get; set; } = [];
}
