using ApplicationLayer.Common;
using ApplicationLayer.DTOs;
using ApplicationLayer.Products.Commands;
using ApplicationLayer.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<OperationResult<IEnumerable<ProductDto>>>> GetAll()
        => Ok(await _mediator.Send(new GetProductsQuery()));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OperationResult<ProductDto>>> GetById(Guid id)
        => Ok(await _mediator.Send(new GetProductByIdQuery(id)));

    [HttpPost]
    public async Task<ActionResult<OperationResult<ProductDto>>> Create([FromBody] CreateProductDto dto)
        => Ok(await _mediator.Send(new CreateProductCommand(dto)));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<OperationResult<ProductDto>>> Update(Guid id, [FromBody] UpdateProductDto dto)
        => Ok(await _mediator.Send(new UpdateProductCommand(id, dto)));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<OperationResult<bool>>> Delete(Guid id)
        => Ok(await _mediator.Send(new DeleteProductCommand(id)));
}
