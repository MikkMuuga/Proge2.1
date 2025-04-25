using Proge2._1.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Services.Interfaces
{
    public interface IMachineService
    {
        Task<PagedResult<Machines>> GetPagedMachines(int page, int pageSize);
        Task<Machines> GetMachineById(int id);
        Task AddMachine(Machines machine);
        Task UpdateMachine(Machines machine);
        Task DeleteMachine(int id);
        Task<bool> MachineExists(int id);
    }
}
