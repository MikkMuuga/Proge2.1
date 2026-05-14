using Proge2._1.Search;

namespace Proge2._1.Data.Repositories
{
    public interface IMaterialsRepository
    {
        Task<PagedResult<Materials>> GetPagedAsync(int page, int pageSize);
        Task<PagedResult<Materials>> GetPagedAsync(int page, int pageSize, MaterialSearch search);
        IQueryable<Materials> GetQueryable();
        Task<Materials> GetByIdAsync(int id);
        Task AddAsync(Materials material);
        Task UpdateAsync(Materials material);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
