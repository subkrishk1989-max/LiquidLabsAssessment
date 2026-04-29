using LiquidLabsAssessment.Application.Features.Products.DTOs;
using LiquidLabsAssessment.Application.Features.Products.Mappers;
using LiquidLabsAssessment.Application.Features.Products.Queries;
using LiquidLabsAssessment.Application.Interfaces;
using MediatR;

namespace LiquidLabsAssessment.Application.Features.Products.Handlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponseDto>>
{
    private readonly IProductRepository _repo;
    private readonly IExternalProductService _external;

    public GetAllProductsHandler(
        IProductRepository repo,
        IExternalProductService external)
    {
        _repo = repo;
        _external = external;
    }

    public async Task<IEnumerable<ProductResponseDto>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _repo.GetAllAsync();

        if (!products.Any())
        {
            products = await _external.GetAllAsync();

            foreach (var p in products)
                await _repo.InsertAsync(p);
        }

        return products.Select(ProductResponseMapper.Map);
    }
}
