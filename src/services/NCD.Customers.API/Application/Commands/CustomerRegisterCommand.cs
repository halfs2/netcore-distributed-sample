using NCD.Core.Messages;
using System;

namespace NCD.Customers.API.Application.Commands
{
    public class CustomerRegisterCommand : Command
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Document { get; set; }

        public CustomerRegisterCommand(Guid id, string name, string email, string document)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Document = document;
        }
    }
}
