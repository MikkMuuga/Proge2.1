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

        public async Task<PagedResult<Machines>> GetPagedAsync(int page, int pageSize)
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
    }
}