using ApplicationLayer.Abstractions;
using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Common;
using ApplicationLayer.DTOs;
using MediatR;

namespace ApplicationLayer.Categories.Commands;

public record UpdateCategoryCommand(Guid Id, UpdateCategoryDto Dto) : IRequest<OperationResult<CategoryDto>>;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, OperationResult<CategoryDto>>
{
    private readonly ICategoryRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateCategoryCommandHandler(ICategoryRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OperationResult<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return OperationResult<CategoryDto>.Fail("Category not found");

        entity.Name = request.Dto.Name;
        await _repo.UpdateAsync(entity);
        await _uow.SaveChangesAsync(ct);

        return OperationResult<CategoryDto>.Ok(new CategoryDto { Id = entity.Id, Name = entity.Name });
    }
}
