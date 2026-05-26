using System.Linq;
using System.Threading.Tasks;
using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Extensions;
using Proge2._1.Search;
using Proge2._1.Services.Interfaces;
using System.Collections.Generic;

namespace Proge2._1.Services
{
    public class MachineService : IMachineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MachineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Machines>> List(int page, int pageSize, MachineSearch search = null)
        {
            search ??= new MachineSearch();

            var query = _unitOfWork.MachinesRepository.GetQueryable();

            if (!string.IsNullOrWhiteSpace(search.Workers))
                query = query.Where(m => m.Workers.Contains(search.Workers));

            if (!string.IsNullOrWhiteSpace(search.Supervision))
                query = query.Where(m => m.Supervision.Contains(search.Supervision));

            if (search.MinCost.HasValue)
                query = query.Where(m => m.CostOfMachines >= search.MinCost.Value);

            if (search.MaxCost.HasValue)
                query = query.Where(m => m.CostOfMachines <= search.MaxCost.Value);

            return await query.OrderBy(m => m.Id).GetPagedAsync(page, pageSize);
        }

        public async Task<PagedResult<Machines>> GetPagedMachines(int page, int pageSize)
        {
            return await _unitOfWork.MachinesRepository.GetPagedAsync(page, pageSize);
        }

        public async Task<PagedResult<Machines>> GetPagedMachines(int page, int pageSize, MachineSearch search)
        {
            return await List(page, pageSize, search);
        }

        public async Task<Machines> GetMachineById(int id)
        {
            return await _unitOfWork.MachinesRepository.GetByIdAsync(id);
        }

        public async Task AddMachine(Machines machine)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.MachinesRepository.AddAsync(machine);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateMachine(Machines machine)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.MachinesRepository.UpdateAsync(machine);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteMachine(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.MachinesRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> MachineExists(int id)
        {
            return await _unitOfWork.MachinesRepository.ExistsAsync(id);
        }
        public async Task Save(Machines machine)
        {
            if (machine.Id == 0)
                await AddMachine(machine);
            else
                await UpdateMachine(machine);
        }
    }
}