using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.DTOs;
using ApplicationLayer.Common;
using MediatR;

namespace ApplicationLayer.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<OperationResult<ProductDto>>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, OperationResult<ProductDto>>
{
    private readonly IProductRepository _repo;

    public GetProductByIdHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<OperationResult<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);
        if (product == null)
            return OperationResult<ProductDto>.Failure("Product not found");

        return OperationResult<ProductDto>.Success(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CategoryId = product.CategoryId
        });
    }
}
