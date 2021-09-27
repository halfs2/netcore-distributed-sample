using NCD.Core.Messages;
using System;

namespace NCD.Customers.API.Application.Commands
{
    public class DeleteCustomerCommand : Command
    {
        public Guid Id { get; private set; }
        
        public DeleteCustomerCommand(Guid id)
        {
            AggregateId = id;
            Id = id;
        }
    }
}
