using ApplicationLayer.Categories.Commands;
using ApplicationLayer.Categories.Queries;
using ApplicationLayer.Common;
using ApplicationLayer.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<OperationResult<IEnumerable<CategoryDto>>>> GetAll()
    {
        var result = await _mediator.Send(new GetCategoriesQuery());
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost] // 👈 detta gör att du kan skapa
    public async Task<ActionResult<OperationResult<CategoryDto>>> Create([FromBody] CreateCategoryDto dto)
    {
        var result = await _mediator.Send(new CreateCategoryCommand(dto));
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
