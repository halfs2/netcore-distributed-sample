using NCD.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCD.Customers.API.Application.Events
{
    public class RegisteredCustomerEvent : Event
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Document { get; set; }

        public RegisteredCustomerEvent(Guid id, string name, string email, string document)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Document = document;
        }
    }
}
