using Proge2._1.Search;

namespace Proge2._1.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id);
        IQueryable<Customer> GetQueryable();
        Task<IEnumerable<Customer>> GetAllAsync();
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<PagedResult<Customer>> GetPagedAsync(int page, int pageSize);
        Task<PagedResult<Customer>> GetPagedAsync(int page, int pageSize, CustomerSearch search);
    }
}
