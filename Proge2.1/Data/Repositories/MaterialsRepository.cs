using Microsoft.EntityFrameworkCore;

namespace Proge2._1.Data.Repositories
{
    public class MaterialsRepository : IMaterialsRepository
    {
        private readonly ApplicationDbContext _context;

        public MaterialsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Materials>> GetPagedAsync(int page, int pageSize)
        {
            var result = new PagedResult<Materials>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await _context.Materials.CountAsync()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await _context.Materials
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<Materials> GetByIdAsync(int id)
        {
            return await _context.Materials.FindAsync(id);
        }

        public async Task AddAsync(Materials material)
        {
            await _context.Materials.AddAsync(material);
        }

        public async Task UpdateAsync(Materials material)
        {
            var existing = await _context.Materials.FindAsync(material.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(material);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var material = await GetByIdAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Materials.AnyAsync(e => e.Id == id);
        }
    }

}
