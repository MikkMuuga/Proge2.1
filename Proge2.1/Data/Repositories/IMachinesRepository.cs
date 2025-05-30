using Proge2._1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Data.Repositories
{
    public interface IMachinesRepository
    {
        Task<PagedResult<Machines>> GetPagedAsync(int page, int pageSize);
        Task<Machines?> GetByIdAsync(int id);
        Task AddAsync(Machines machine);
        Task UpdateAsync(Machines machine);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}