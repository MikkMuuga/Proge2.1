using Proge2._1.Data;
using Proge2._1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly ApplicationDbContext _context;

        public MaterialService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Materials>> GetPagedMaterials(int page, int pageSize)
        {
            return await _context.Materials
                .OrderBy(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Materials> GetMaterialById(int id)
        {
            return await _context.Materials.FindAsync(id);
        }

        public async Task AddMaterial(Materials material)
        {
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMaterial(Materials material)
        {
            _context.Entry(material).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MaterialExists(int id)
        {
            return await _context.Materials.AnyAsync(e => e.Id == id);
        }
    }
}
