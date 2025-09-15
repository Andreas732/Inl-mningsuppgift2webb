using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Common;
using ApplicationLayer.DTOs;
using MediatR;

namespace ApplicationLayer.Categories.Queries;

public record GetCategoriesQuery() : IRequest<OperationResult<IEnumerable<CategoryDto>>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, OperationResult<IEnumerable<CategoryDto>>>
{
    private readonly ICategoryRepository _repo;
    public GetCategoriesQueryHandler(ICategoryRepository repo) => _repo = repo;

    public async Task<OperationResult<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        var items = await _repo.GetAllAsync();
        var dtos = items.Select(c => new CategoryDto { Id = c.Id, Name = c.Name });
        return OperationResult<IEnumerable<CategoryDto>>.Ok(dtos);
    }
}
