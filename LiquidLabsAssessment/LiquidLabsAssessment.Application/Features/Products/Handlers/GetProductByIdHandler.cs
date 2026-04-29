using LiquidLabsAssessment.Application.Features.Products.DTOs;
using LiquidLabsAssessment.Application.Features.Products.Mappers;
using LiquidLabsAssessment.Application.Features.Products.Queries;
using LiquidLabsAssessment.Application.Interfaces;
using MediatR;

namespace LiquidLabsAssessment.Application.Features.Products.Handlers;

public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
{
    private readonly IProductRepository _repo;
    private readonly IExternalProductService _external;

    public GetProductByIdHandler(
        IProductRepository repo,
        IExternalProductService external)
    {
        _repo = repo;
        _external = external;
    }

    public async Task<ProductResponseDto> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);

        if (product == null)
        {
            product = await _external.GetByIdAsync(request.Id);

            if (product == null)
                throw new Exception("Product not found");

            await _repo.InsertAsync(product);
        }

        return ProductResponseMapper.Map(product);
    }
}
