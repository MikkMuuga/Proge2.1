using Proge2._1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            customer.Date = DateTime.UtcNow; // Set current date if not provided

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(e => e.CustomerId == id);
        }

        public async Task<PagedResult<Customer>> GetPagedCustomers(int page, int pageSize)
        {
            return await _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.CustomerId)  // or any order you want
                .GetPagedAsync(page, pageSize);
        }


        public async Task AddCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            customer.Date = DateTime.UtcNow;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<bool> CustomerExists(int customerId)
        {
            return await _context.Customers.AnyAsync(c => c.CustomerId == customerId);
        }

        Task<string?> ICustomerService.GetCustomerById(int value)
        {
            throw new NotImplementedException();
        }
    }
}
