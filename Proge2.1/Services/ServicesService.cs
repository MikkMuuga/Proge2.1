using Proge2._1.Data;
using Proge2._1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public class ServicesService : IServicessService
    {
        private readonly ApplicationDbContext _context;

        public ServicesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResults<Servicess>> GetPagedServices(int page, int pageSize)
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

        public async Task<Servicess> GetServiceById(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == id);
        }

        public async Task AddService(Servicess service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateService(Servicess service)
        {
            _context.Entry(service).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ServiceExists(int id)
        {
            return await _context.Services.AnyAsync(e => e.ServiceId == id);
        }
    }
}