using Microsoft.EntityFrameworkCore;
using Proge2._1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proge2._1.Data.Repositories
{
    public class MachinesRepository : IMachinesRepository
    {
        private readonly ApplicationDbContext _context;

        public MachinesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Machines>> GetPagedAsync(int page, int pageSize, Search.MachineSearch search)
        {
            var result = new PagedResult<Machines>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await _context.Machines.CountAsync()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)System.Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await _context.Machines.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }

        // Implemented to satisfy IMachinesRepository.GetPagedAsync(int, int)
        public async Task<PagedResult<Machines>> GetPagedAsync(int page, int pageSize)
        {
            // Delegate to the search overload with a default search
            return await GetPagedAsync(page, pageSize, new Search.MachineSearch());
        }

        public async Task<Machines?> GetByIdAsync(int id)
        {
            return await _context.Machines.FindAsync(id);
        }

        public async Task AddAsync(Machines machine)
        {
            await _context.Machines.AddAsync(machine);
        }

        public async Task UpdateAsync(Machines machine)
        {
            _context.Machines.Update(machine);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var machine = await GetByIdAsync(id);
            if (machine != null)
            {
                _context.Machines.Remove(machine);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Machines.AnyAsync(e => e.Id == id);
        }

        // Implemented to satisfy IMachinesRepository.GetAllAsync()
        public async Task GetAllAsync()
        {
            // Materialize all machines (result intentionally not returned to match interface signature)
            await _context.Machines.ToListAsync();
        }
        public IQueryable<Machines> GetQueryable()
        {
            return _context.Machines.AsQueryable();
        }
    }
}