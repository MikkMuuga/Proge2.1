using Microsoft.EntityFrameworkCore;
using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Extensions;
using Proge2._1.Search;
using Proge2._1.Services.Interfaces;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public class ServicesService : IServicessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Servicess>> List(int page, int pageSize, ServiceSearch search = null)
        {
            search ??= new ServiceSearch();

            var query = _unitOfWork.ServicesRepository.GetQueryable();

            if (!string.IsNullOrWhiteSpace(search.Transportation))
                query = query.Where(s => s.transportation.Contains(search.Transportation));

            if (search.PanelProduction.HasValue)
                query = query.Where(s => s.PanelProduction == search.PanelProduction.Value);

            if (!string.IsNullOrWhiteSpace(search.Montage))
                query = query.Where(s => s.montage.Contains(search.Montage));

            query = query.OrderBy(s => s.ServiceId);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Servicess>
            {
                Items = items,
                TotalCount = total,
                TotalItems = total,
                CurrentPage = page,
                PageNumber = page,
                PageSize = pageSize,
                PageCount = pageSize == 0 ? 0 : (int)Math.Ceiling((double)total / pageSize)
            };
        }

        public async Task<PagedResult<Servicess>> GetPagedServices(int page, int pageSize)
        {
            return await List(page, pageSize);
        }

        public async Task<PagedResult<Servicess>> GetPagedServices(int page, int pageSize, ServiceSearch search)
        {
            return await List(page, pageSize, search);
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