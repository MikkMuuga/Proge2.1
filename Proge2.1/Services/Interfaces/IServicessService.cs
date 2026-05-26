using Proge2._1.Controllers;
using Proge2._1.Data;
using Proge2._1.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Services.Interfaces
{
    public interface IServicessService
    {
        Task<PagedResult<Servicess>> GetPagedServices(int page, int pageSize);
        Task<PagedResult<Servicess>> GetPagedServices(int page, int pageSize, ServiceSearch search);
        Task<Servicess> GetServiceById(int id);
        Task AddService(Servicess service);
        Task UpdateService(Servicess service);
        Task DeleteService(int id);
        Task Save(Servicess service);
        Task<bool> ServiceExists(int id);
    }
}
