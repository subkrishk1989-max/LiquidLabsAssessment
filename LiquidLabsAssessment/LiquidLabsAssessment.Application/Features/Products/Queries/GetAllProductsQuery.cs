using LiquidLabsAssessment.Application.Features.Products.DTOs;
using MediatR;

namespace LiquidLabsAssessment.Application.Features.Products.Queries;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductResponseDto>>;
