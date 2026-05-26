using Proge2._1.Data;
using Proge2._1.Search;
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
        Task DeleteCustomer(int id);
        Task<Customer?> GetCustomerById(int value);
        Task<bool> CustomerExists(int customerId);
        Task UpdateCustomer(Customer customer);
        Task AddCustomer(Customer customer);
        Task Save(Customer customer);
        Task<PagedResult<Customer>> GetPagedCustomers(int page, int pageSize);
        Task<PagedResult<Customer>> GetPagedCustomers(int page, int pageSize, CustomerSearch search);

    }
}