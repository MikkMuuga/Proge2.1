using Microsoft.EntityFrameworkCore;

namespace Proge2._1.Data.Repositories
{
    public class ServicessRepository : IServicessRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResults<Servicess>> GetPagedAsync(int page, int pageSize)
        {
            var items = await _context.Services
                .OrderBy(s => s.ServiceId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.Services.CountAsync();

            return new PagedResults<Servicess>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<Servicess> GetByIdAsync(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == id);
        }

        public async Task AddAsync(Servicess service)
        {
            await _context.Services.AddAsync(service);
        }

        public async Task UpdateAsync(Servicess service)
        {
            var existing = await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == service.ServiceId);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(service);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var service = await GetByIdAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Services.AnyAsync(e => e.ServiceId == id);
        }
    }

}
