using ApplicationLayer.Abstractions;
using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Common;
using ApplicationLayer.DTOs;
using DomainLayer.Entities;
using MediatR;

namespace ApplicationLayer.Categories.Commands;

public record CreateCategoryCommand(CreateCategoryDto Dto) : IRequest<OperationResult<CategoryDto>>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, OperationResult<CategoryDto>>
{
    private readonly ICategoryRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateCategoryCommandHandler(ICategoryRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OperationResult<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var entity = new Category { Id = Guid.NewGuid(), Name = request.Dto.Name };
        await _repo.AddAsync(entity);
        await _uow.SaveChangesAsync(ct);

        return OperationResult<CategoryDto>.Ok(new CategoryDto { Id = entity.Id, Name = entity.Name });
    }
}
