using NCD.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCD.Customers.API.Model
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Add(Customer customer);
    }
}
