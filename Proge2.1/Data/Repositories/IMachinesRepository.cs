using Proge2._1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Data.Repositories
{
    public interface IMachinesRepository
    {
        Task<PagedResult<Machines>> GetPagedAsync(int page, int pageSize, Search.MachineSearch search);
        Task<Machines?> GetByIdAsync(int id);
        IQueryable<Machines> GetQueryable();
        Task AddAsync(Machines machine);
        Task UpdateAsync(Machines machine);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task GetAllAsync();
        Task<PagedResult<Machines>> GetPagedAsync(int page, int pageSize);
    }
}