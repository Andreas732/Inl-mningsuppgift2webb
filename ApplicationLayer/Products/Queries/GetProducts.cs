using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.DTOs;
using ApplicationLayer.Common;
using MediatR;

namespace ApplicationLayer.Products.Queries;

public record GetProductsQuery() : IRequest<OperationResult<IEnumerable<ProductDto>>>;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, OperationResult<IEnumerable<ProductDto>>>
{
    private readonly IProductRepository _repo;

    public GetProductsHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<OperationResult<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repo.GetAllAsync();
        var dtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            CategoryId = p.CategoryId
        }).ToList();

        return OperationResult<IEnumerable<ProductDto>>.Success(dtos);
    }
}
