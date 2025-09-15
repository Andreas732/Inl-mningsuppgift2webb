using ApplicationLayer.Common;
using ApplicationLayer.Abstractions.Repositories;
using DomainLayer.Entities;
using MediatR;
using ApplicationLayer.Abstractions;

namespace ApplicationLayer.Customers.Commands;

public record DeleteCustomerCommand(Guid Id) : IRequest<OperationResult<string>>;

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, OperationResult<string>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
            return OperationResult<string>.Fail("Customer not found");

        await _customerRepository.DeleteAsync(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return OperationResult<string>.Ok("Customer deleted successfully");
    }
}
