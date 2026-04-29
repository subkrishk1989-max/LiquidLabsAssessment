using Newtonsoft.Json.Linq;

namespace LiquidLabsAssessment.Infrastructure.External.DTOs;

public class ProductApiResponse
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";

    public JObject? Data { get; set; }
}
