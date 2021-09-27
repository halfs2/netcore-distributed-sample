using Microsoft.EntityFrameworkCore;
using NCD.Core.Data;
using NCD.Customers.API.Model;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NCD.Customers.API.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Exists(Expression<Func<Customer, bool>> predicate)
        {
            return await _context.Customers.AnyAsync(predicate);
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void Remove(Guid id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
                return;

            _context.Customers.Remove(customer);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

       
    }
}
