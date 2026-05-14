using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Models;
using Proge2._1.Search;
using Proge2._1.Extensions;
using Proge2._1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Customer>> List(int page, int pageSize, CustomerSearch search = null)
        {
            search ??= new CustomerSearch();

            var query = _unitOfWork.CustomerRepository.GetQueryable();

            if (!string.IsNullOrWhiteSpace(search.Name))
                query = query.Where(c => c.Name.Contains(search.Name));

            if (search.DateFrom.HasValue)
                query = query.Where(c => c.Date >= search.DateFrom.Value);

            if (search.DateTo.HasValue)
                query = query.Where(c => c.Date <= search.DateTo.Value);

            if (!string.IsNullOrWhiteSpace(search.Contact))
                query = query.Where(c => c.Contact.Contains(search.Contact));

            return await query.OrderBy(c => c.Id).GetPagedAsync(page, pageSize);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _unitOfWork.CustomerRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _unitOfWork.CustomerRepository.GetAllAsync();
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            customer.Date = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.CustomerRepository.AddAsync(customer);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
                return customer;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.CustomerRepository.UpdateAsync(customer);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.CustomerRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CustomerExistsAsync(int id)
        {
            return await _unitOfWork.CustomerRepository.ExistsAsync(id);
        }

        public async Task<PagedResult<Customer>> GetPagedCustomers(int page, int pageSize)
        {
            return await _unitOfWork.CustomerRepository.GetPagedAsync(page, pageSize);
        }

        public async Task AddCustomer(Customer customer)
        {
            await CreateCustomerAsync(customer);
        }

        public async Task UpdateCustomer(Customer customer)
        {
            await UpdateCustomerAsync(customer);
        }

        public async Task DeleteCustomer(int id)
        {
            await DeleteCustomerAsync(id);
        }

        public async Task<Customer?> GetCustomerById(int value)
        {
            return await GetCustomerByIdAsync(value);
        }

        public async Task<bool> CustomerExists(int customerId)
        {
            return await CustomerExistsAsync(customerId);
        }

        public async Task<PagedResult<Customer>> GetPagedCustomers(int page, int pageSize, CustomerSearch search)
        {
            return await _unitOfWork.CustomerRepository.GetPagedAsync(page, pageSize, search);
        }
    }
}