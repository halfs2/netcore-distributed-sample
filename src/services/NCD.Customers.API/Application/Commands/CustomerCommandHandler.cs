using FluentValidation.Results;
using MediatR;
using NCD.Core.Messages;
using NCD.Customers.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NCD.Customers.API.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler, IRequestHandler<CustomerRegisterCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(CustomerRegisterCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var customer = new Customer(message.Id, message.Name, message.Email, message.Document);

            if(true)
                AddError("any error");

            return message.ValidationResult;
        }
    }
}
