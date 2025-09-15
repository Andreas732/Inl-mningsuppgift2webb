using ApplicationLayer.Common;
using ApplicationLayer.Customers.DTOs;
using ApplicationLayer.Abstractions.Repositories;
using DomainLayer.Entities;
using MediatR;
using ApplicationLayer.Abstractions;
using ApplicationLayer.DTOs;

namespace ApplicationLayer.Customers.Commands;

public record UpdateCustomerCommand(Guid Id, UpdateCustomerDto Dto) : IRequest<OperationResult<CustomerDto>>;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, OperationResult<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
            return OperationResult<CustomerDto>.Fail("Customer not found");

        customer.FirstName = request.Dto.FirstName;
        customer.LastName = request.Dto.LastName;
        customer.Email = request.Dto.Email;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
