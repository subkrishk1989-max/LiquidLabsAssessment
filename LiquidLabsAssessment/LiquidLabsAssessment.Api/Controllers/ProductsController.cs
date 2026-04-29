using LiquidLabsAssessment.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LiquidLabsAssessment.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllProductsQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
        => Ok(await _mediator.Send(new GetProductByIdQuery(id)));
}
