using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Common;
using ApplicationLayer.DTOs;
using MediatR;

namespace ApplicationLayer.Categories.Queries;

public record GetCategoryByIdQuery(Guid Id) : IRequest<OperationResult<CategoryDto>>;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, OperationResult<CategoryDto>>
{
    private readonly ICategoryRepository _repo;
    public GetCategoryByIdQueryHandler(ICategoryRepository repo) => _repo = repo;

    public async Task<OperationResult<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return OperationResult<CategoryDto>.Fail("Category not found");

        return OperationResult<CategoryDto>.Ok(new CategoryDto { Id = entity.Id, Name = entity.Name });
    }
}
