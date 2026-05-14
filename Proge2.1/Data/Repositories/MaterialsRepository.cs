using Microsoft.EntityFrameworkCore;
using Proge2._1.Search;

namespace Proge2._1.Data.Repositories
{
    public class MaterialsRepository : IMaterialsRepository
    {
        private readonly ApplicationDbContext _context;

        public MaterialsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Materials>> GetPagedAsync(int page, int pageSize, MaterialSearch search)
        {
            IQueryable<Materials> query = _context.Materials.AsNoTracking();

            if (!string.IsNullOrEmpty(search?.Unit))
                query = query.Where(m => m.Unit.Contains(search.Unit));

            if (!string.IsNullOrEmpty(search?.Seller))
                query = query.Where(m => m.Seller.Contains(search.Seller));

            if (search?.MinPrice.HasValue == true)
                query = query.Where(m => m.Price >= search.MinPrice.Value);

            if (search?.MaxPrice.HasValue == true)
                query = query.Where(m => m.Price <= search.MaxPrice.Value);

            query = query.OrderBy(m => m.Id);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Materials>
            {
                Results = items,
                TotalCount = total,
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling((double)total / pageSize)
            };
        }

        public async Task<PagedResult<Materials>> GetPagedAsync(int page, int pageSize)
        {
            return await GetPagedAsync(page, pageSize, new MaterialSearch());
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
        public IQueryable<Materials> GetQueryable()
        {
            return _context.Materials.AsQueryable();
        }
    }

}
