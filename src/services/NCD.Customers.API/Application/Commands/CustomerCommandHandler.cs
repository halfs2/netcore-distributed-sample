using FluentValidation.Results;
using MediatR;
using NCD.Core.Messages;
using NCD.Customers.API.Application.Events;
using NCD.Customers.API.Model;
using System.Threading;
using System.Threading.Tasks;

namespace NCD.Customers.API.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler, IRequestHandler<CustomerRegisterCommand, ValidationResult>
    {
        private readonly ICustomerRepository _repository;
        public CustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<ValidationResult> Handle(CustomerRegisterCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var customer = new Customer(message.Id, message.Name, message.Email, message.Document);

            if (await _repository.Exists(c => c.Document == customer.Document))
            {
                AddError("Already exist a customer with this document");
                return ValidationResult;
            }

            _repository.Add(customer);

            customer.AddEvent(new RegisteredCustomerEvent(message.Id, message.Name, message.Email, message.Document));

            return await PersistData(_repository.UnitOfWork);
        }
    }
}
