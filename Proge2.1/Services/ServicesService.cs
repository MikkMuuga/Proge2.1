
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Proge2._1.Data.Repositories;

namespace Proge2._1.Services
{
    public class ServicesService : IServicessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResults<Servicess>> GetPagedServices(int page, int pageSize)
        {
            return await _unitOfWork.ServicesRepository.GetPagedAsync(page, pageSize);
        }

        public async Task<Servicess> GetServiceById(int id)
        {
            return await _unitOfWork.ServicesRepository.GetByIdAsync(id);
        }

        public async Task AddService(Servicess service)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.ServicesRepository.AddAsync(service);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateService(Servicess service)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.ServicesRepository.UpdateAsync(service);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteService(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.ServicesRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ServiceExists(int id)
        {
            return await _unitOfWork.ServicesRepository.ExistsAsync(id);
        }
    }
}
