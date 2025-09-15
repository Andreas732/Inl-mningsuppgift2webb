using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.DTOs;
using ApplicationLayer.Common;
using MediatR;
using ApplicationLayer.Abstractions;

namespace ApplicationLayer.Products.Commands;

public record UpdateProductCommand(Guid Id, UpdateProductDto Dto) : IRequest<OperationResult<ProductDto>>;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, OperationResult<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateProductHandler(IProductRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OperationResult<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);
        if (product == null)
            return OperationResult<ProductDto>.Failure("Product not found");

        product.Name = request.Dto.Name;
        product.Price = request.Dto.Price;
        product.CategoryId = request.Dto.CategoryId;

        await _uow.SaveChangesAsync(cancellationToken);

        return OperationResult<ProductDto>.Success(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CategoryId = product.CategoryId
        });
    }
}
