using Microsoft.EntityFrameworkCore;
using Proge2._1.Search;


namespace Proge2._1.Data.Repositories
{
    public class ServicessRepository : IServicessRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Servicess>> GetPagedAsync(int page, int pageSize)
        {
            return await GetPagedAsync(page, pageSize, new ServiceSearch());
        }

        public async Task<PagedResult<Servicess>> GetPagedAsync(int page, int pageSize, ServiceSearch search)
        {
            IQueryable<Servicess> query = _context.Services.AsNoTracking();
            if (!string.IsNullOrEmpty(search?.Transportation))
                query = query.Where(s => s.transportation.Contains(search.Transportation));

            if (search?.PanelProduction.HasValue == true)
                query = query.Where(s => s.PanelProduction == search.PanelProduction.Value);

            if (!string.IsNullOrEmpty(search?.Montage))
                query = query.Where(s => s.montage.Contains(search.Montage));

            query = query.OrderBy(s => s.ServiceId);

            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Servicess>
            {
                Items = items,
                TotalCount = totalCount,
                TotalItems = totalCount,
                PageNumber = page,
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = pageSize == 0 ? 0 : (int)Math.Ceiling((decimal)totalCount / pageSize)
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
        public IQueryable<Servicess> GetQueryable()
        {
            return _context.Services.AsQueryable();
        }
    }

}
