using NCD.Core.DomainObjects;
using System;

namespace NCD.Customers.API.Model
{
    public class Address : Entity
    {
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
        public Guid CustomerId { get; private set; }

        // EF Relation
        public Customer Customer { get; protected set; }

        public Address(string street, string number, string city, string state, string country, string zipCode, Guid customerId)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
            CustomerId = customerId;
            Number = number;
        }

        //EF Constructor
        protected Address() { }
    }
}
