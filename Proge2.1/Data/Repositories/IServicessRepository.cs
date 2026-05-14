using Proge2._1.Search;

namespace Proge2._1.Data.Repositories
{
    public interface IServicessRepository
    {
        Task<PagedResults<Servicess>> GetPagedAsync(int page, int pageSize);
        Task<PagedResults<Servicess>> GetPagedAsync(int page, int pageSize, ServiceSearch search);
        IQueryable<Servicess> GetQueryable();
        Task<Servicess> GetByIdAsync(int id);
        Task AddAsync(Servicess service);
        Task UpdateAsync(Servicess service);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
