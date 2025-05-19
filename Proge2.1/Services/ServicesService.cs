
using Proge2._1.Data;
using Proge2._1.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proge2._1.Controllers;
using Proge2._1.Services.Interfaces;

namespace Proge2._1.Services
{
    public class ServicesService : IServicessService
    {
        private readonly ApplicationDbContext _context;

        public ServicesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicess>> GetPagedServices(int page, int pageSize)
        {
            return await _context.Services
                .OrderBy(s => s.ServiceId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Data.Servicess> GetServiceById(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == id);
        }

        public async Task AddService(Data.Servicess service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateService(Data.Servicess service)
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

    public interface IServicessService
    {
    }
}
