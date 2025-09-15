using ApplicationLayer.Abstractions;
using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Common;
using ApplicationLayer.Customers.DTOs;
using ApplicationLayer.DTOs;
using DomainLayer.Entities;
using MediatR;

namespace ApplicationLayer.Customers.Commands;

public record CreateCustomerCommand(CreateCustomerDto Dto) : IRequest<OperationResult<CustomerDto>>;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, OperationResult<CustomerDto>>
{
    private readonly ICustomerRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateCustomerHandler(ICustomerRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OperationResult<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = request.Dto.FirstName,
            LastName = request.Dto.LastName,
            Email = request.Dto.Email,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(customer);
        await _uow.SaveChangesAsync(cancellationToken);

        var dto = new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email
        };

        return OperationResult<CustomerDto>.Ok(dto);
    }
}
