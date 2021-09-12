using NCD.Core.Data;
using NCD.Customers.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCD.Customers.API.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Customer customer)
        {
            _context.Add(customer);
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
