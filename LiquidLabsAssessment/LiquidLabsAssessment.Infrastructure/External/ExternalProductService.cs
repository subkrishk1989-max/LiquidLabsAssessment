using LiquidLabsAssessment.Application.Interfaces;
using LiquidLabsAssessment.Domain.Entities;
using LiquidLabsAssessment.Infrastructure.Configurations;
using LiquidLabsAssessment.Infrastructure.External.DTOs;
using LiquidLabsAssessment.Infrastructure.Mappers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LiquidLabsAssessment.Infrastructure.External;

internal class ExternalProductService : IExternalProductService
{
    private readonly HttpClient _httpClient;
    private readonly ExternalApiSettings _settings;
    public ExternalProductService(HttpClient httpClient,
        IOptions<ExternalApiSettings> options)
    {
       _httpClient = httpClient;
       _settings = options.Value;
    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            var url = $"{_settings.BaseUrl}{_settings.ProductsEndpoint}";
            var response = await _httpClient.GetStringAsync(url);
            var dtos = JsonConvert.DeserializeObject<List<ProductApiResponse>>(response) 
                ?? [];

            return dtos.Select(ProductMapper.Map);

        }
        catch (HttpRequestException ex)
        {

            throw new Exception("External API unavailable", ex);
        }
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        try
        {
            var url =
            $"{_settings.BaseUrl}{_settings.ProductsEndpoint}/{id}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            var dto = JsonConvert.DeserializeObject<ProductApiResponse>(json);

            return dto == null
                ? null
                : ProductMapper.Map(dto);
        }
        catch (HttpRequestException ex)
        {

            throw new Exception("External API unavailable", ex);
        }
    }
}
