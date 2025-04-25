using Proge2._1.Controllers;
using Proge2._1.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Servicess = Proge2._1.Controllers.Servicess;

namespace Proge2._1.Services.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<Servicess>> GetPagedServices(int page, int pageSize);
        Task<Servicess> GetServiceById(int id);
        Task AddService(Servicess service);
        Task UpdateService(Servicess service);
        Task DeleteService(int id);
        Task<bool> ServiceExists(int id);
    }
}
