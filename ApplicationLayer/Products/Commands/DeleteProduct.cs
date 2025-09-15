using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Common;
using MediatR;
using ApplicationLayer.Abstractions;

namespace ApplicationLayer.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<OperationResult<bool>>;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, OperationResult<bool>>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteProductHandler(IProductRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OperationResult<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);
        if (product == null)
            return OperationResult<bool>.Fail("Product not found");

        await _repo.DeleteAsync(product); // ✅ rätt metod
        await _uow.SaveChangesAsync(cancellationToken);

        return OperationResult<bool>.Ok(true);
    }
}