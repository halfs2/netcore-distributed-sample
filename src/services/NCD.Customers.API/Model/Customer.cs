using NCD.Core.DomainObjects;
using NCD.Customers.API.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCD.Customers.API.Model
{
    public class Customer: Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public string Document { get; set; }
        public bool Deleted { get; private set; }
        public Address Address { get; private set; }

        public Customer(Guid id, string name, string email, string document)
        {
            Id = id;
            Name = name;
            Email = new Email(email);
            Document = document;
            Deleted = false;
        }

        //EF Constructor
        protected Customer() { }
    }
}
