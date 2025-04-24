using Proge2._1.Data;
using Proge2._1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public class MachineService : IMachineService
    {
        private readonly ApplicationDbContext _context;

        public MachineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Machines>> GetPagedMachines(int page, int pageSize)
        {
            return await _context.Machines
                .OrderBy(m => m.id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Machines> GetMachineById(int id)
        {
            return await _context.Machines.FindAsync(id);
        }

        public async Task AddMachine(Machines machine)
        {
            _context.Machines.Add(machine);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMachine(Machines machine)
        {
            _context.Entry(machine).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMachine(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine != null)
            {
                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MachineExists(int id)
        {
            return await _context.Machines.AnyAsync(e => e.id == id);
        }
    }
}