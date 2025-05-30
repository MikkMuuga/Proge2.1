using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Models;
using Proge2._1.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Proge2._1.Services
{
    public class MachineService : IMachineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MachineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Machines>> GetPagedMachines(int page, int pageSize)
        {
            return await _unitOfWork.MachinesRepository.GetPagedAsync(page, pageSize);
        }

        public async Task<Machines?> GetMachineById(int id)
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
    }
}
