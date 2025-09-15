using ApplicationLayer.Abstractions.Repositories;
using ApplicationLayer.Customers.DTOs;
using ApplicationLayer.Common;
using MediatR;
using ApplicationLayer.DTOs;

namespace ApplicationLayer.Customers.Queries;

public record GetCustomerByIdQuery(Guid Id) : IRequest<OperationResult<CustomerDto>>;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, OperationResult<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<OperationResult<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
            return OperationResult<CustomerDto>.Fail("Customer not found");

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
