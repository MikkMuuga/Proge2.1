using Microsoft.EntityFrameworkCore;
using Proge2._1.Models;
using Proge2._1.Search;

namespace Proge2._1.Data.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id);
        }

        public async Task<PagedResult<Customer>> GetPagedAsync(int page, int pageSize, CustomerSearch search)
        {
            IQueryable<Customer> query = _context.Customers.AsNoTracking();

            if (!string.IsNullOrEmpty(search?.Name))
                query = query.Where(c => c.Name.Contains(search.Name));

            if (search?.DateFrom.HasValue == true)
                query = query.Where(c => c.Date >= search.DateFrom.Value);

            if (search?.DateTo.HasValue == true)
                query = query.Where(c => c.Date <= search.DateTo.Value);

            if (!string.IsNullOrEmpty(search?.Contact))
                query = query.Where(c => c.Contact.Contains(search.Contact));

            query = query.OrderBy(c => c.Id);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Customer>
            {
                Results = items,
                TotalCount = total,
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling((double)total / pageSize)
            };
        }
        public async Task<PagedResult<Customer>> GetPagedAsync(int page, int pageSize)
        {
            return await GetPagedAsync(page, pageSize, new CustomerSearch());
        }
        public IQueryable<Customer> GetQueryable()
        {
            return _context.Customers.AsQueryable();
        }
    }
}
