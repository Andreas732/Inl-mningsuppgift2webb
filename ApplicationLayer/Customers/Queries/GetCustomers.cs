using ApplicationLayer.Common;
using ApplicationLayer.Customers.DTOs;
using ApplicationLayer.Abstractions.Repositories;
using MediatR;
using ApplicationLayer.DTOs;

namespace ApplicationLayer.Customers.Queries;

public record GetCustomersQuery() : IRequest<OperationResult<IEnumerable<CustomerDto>>>;

public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, OperationResult<IEnumerable<CustomerDto>>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<OperationResult<IEnumerable<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetAllAsync();

        var dtos = customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email
        });

        return OperationResult<IEnumerable<CustomerDto>>.Ok(dtos);
    }
}
