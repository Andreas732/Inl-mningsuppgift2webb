using ApplicationLayer.Abstractions;
using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Common;
using ApplicationLayer.DTOs;
using DomainLayer.Entities;
using MediatR;

namespace ApplicationLayer.Products.Commands;

public record CreateProductCommand(CreateProductDto Dto) : IRequest<OperationResult<ProductDto>>;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, OperationResult<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateProductHandler(IProductRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OperationResult<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Dto.Name,
            Description = request.Dto.Description,
            Price = request.Dto.Price,
            CategoryId = request.Dto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(product);
        await _uow.SaveChangesAsync(cancellationToken);

        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId
        };

        return OperationResult<ProductDto>.Ok(dto);
    }
}
