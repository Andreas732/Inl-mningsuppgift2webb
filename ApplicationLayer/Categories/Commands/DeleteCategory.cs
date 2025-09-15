using ApplicationLayer.Abstractions;
using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Common;
using MediatR;

namespace ApplicationLayer.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest<OperationResult<bool>>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OperationResult<bool>>
{
    private readonly ICategoryRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteCategoryCommandHandler(ICategoryRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OperationResult<bool>> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return OperationResult<bool>.Fail("Category not found");

        await _repo.DeleteAsync(entity);
        await _uow.SaveChangesAsync(ct);

        return OperationResult<bool>.Ok(true);
    }
}
