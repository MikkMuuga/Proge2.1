using Proge2._1.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
        Task<bool> CustomerExistsAsync(int id);
        Task<string?> GetPagedCustomers(int page, int pageSize);
        Task DeleteCustomer(int id);
        Task<string?> GetCustomerById(int value);
        Task<bool> CustomerExists(int customerId);
        Task UpdateCustomer(Customer customer);
        Task AddCustomer(Customer customer);
    }
}