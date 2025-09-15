using ApplicationLayer.Common;
using ApplicationLayer.Customers.Commands;
using ApplicationLayer.Customers.DTOs;
using ApplicationLayer.Customers.Queries;
using ApplicationLayer.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<OperationResult<IEnumerable<CustomerDto>>>> GetAll()
    {
        var result = await _mediator.Send(new GetCustomersQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OperationResult<CustomerDto>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id));
        if (!result.IsSuccess || result.Data == null)
            return NotFound(OperationResult<CustomerDto>.Fail("Customer not found"));

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OperationResult<CustomerDto>>> Create([FromBody] CreateCustomerDto dto)
    {
        var result = await _mediator.Send(new CreateCustomerCommand(dto));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OperationResult<CustomerDto>>> Update(Guid id, [FromBody] UpdateCustomerDto dto)
    {
        var result = await _mediator.Send(new UpdateCustomerCommand(id, dto));
        if (!result.IsSuccess || result.Data == null)
            return NotFound(OperationResult<CustomerDto>.Fail("Customer not found"));

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<OperationResult<string>>> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteCustomerCommand(id));
        if (!result.IsSuccess)
            return NotFound(OperationResult<string>.Fail("Customer not found"));

        return Ok(result);
    }
}