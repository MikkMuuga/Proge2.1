using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Search;
using Proge2._1.Services;

namespace Proge.UnitTests.ServiceTests
{
    public class CustomerServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.CustomerRepository).Returns(_customerRepositoryMock.Object);
            _service = new CustomerService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_should_return_customer()
        {
            var customer = new Customer { Id = 1, Name = "Test", Contact = "Contact 1", Date = DateTime.Now };
            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(customer);

            var result = await _service.GetCustomerByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(customer, result);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_should_return_null_when_not_found()
        {
            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Customer?)null);

            var result = await _service.GetCustomerByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllCustomersAsync_should_return_all_customers()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Test 1", Contact = "Contact 1", Date = DateTime.Now },
                new Customer { Id = 2, Name = "Test 2", Contact = "Contact 2", Date = DateTime.Now }
            };
            _customerRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(customers);

            var result = await _service.GetAllCustomersAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateCustomerAsync_should_throw_when_customer_is_null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateCustomerAsync(null));
        }

        [Fact]
        public async Task CreateCustomerAsync_should_call_repository_and_save()
        {
            var customer = new Customer { Id = 1, Name = "Test", Contact = "Contact 1", Date = DateTime.Now };
            _customerRepositoryMock.Setup(x => x.AddAsync(customer)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.CreateCustomerAsync(customer);

            _customerRepositoryMock.Verify(x => x.AddAsync(customer), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerAsync_should_throw_when_customer_is_null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateCustomerAsync(null));
        }

        [Fact]
        public async Task UpdateCustomerAsync_should_call_repository_and_save()
        {
            var customer = new Customer { Id = 1, Name = "Test", Contact = "Contact 1", Date = DateTime.Now };
            _customerRepositoryMock.Setup(x => x.UpdateAsync(customer)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.UpdateCustomerAsync(customer);

            _customerRepositoryMock.Verify(x => x.UpdateAsync(customer), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomerAsync_should_call_repository_and_save()
        {
            _customerRepositoryMock.Setup(x => x.DeleteAsync(1)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.DeleteCustomerAsync(1);

            _customerRepositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CustomerExistsAsync_should_return_true_when_exists()
        {
            _customerRepositoryMock
                .Setup(x => x.ExistsAsync(1))
                .ReturnsAsync(true);

            var result = await _service.CustomerExistsAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task CustomerExistsAsync_should_return_false_when_not_found()
        {
            _customerRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _service.CustomerExistsAsync(99);

            Assert.False(result);
        }

        [Fact]
        public async Task GetPagedCustomers_should_return_paged_results()
        {
            var pagedResult = new PagedResult<Customer>
            {
                Results = new List<Customer>
                {
                    new Customer { Id = 1, Name = "Test 1", Contact = "Contact 1", Date = DateTime.Now },
                    new Customer { Id = 2, Name = "Test 2", Contact = "Contact 2", Date = DateTime.Now }
                },
                TotalItems = 2
            };
            _customerRepositoryMock
                .Setup(x => x.GetPagedAsync(1, 10))
                .ReturnsAsync(pagedResult);

            var result = await _service.GetPagedCustomers(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
        }
        [Fact]
        public async Task Save_should_add_new_customer_when_id_is_zero()
        {
            var customer = new Customer { Id = 0, Name = "New Customer", Contact = "Contact 1", Date = DateTime.Now };
            _customerRepositoryMock.Setup(x => x.AddAsync(customer)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(customer);

            _customerRepositoryMock.Verify(x => x.AddAsync(customer), Times.Once);
        }

        [Fact]
        public async Task Save_should_update_existing_customer_when_id_is_not_zero()
        {
            var customer = new Customer { Id = 1, Name = "Existing Customer", Contact = "Contact 1", Date = DateTime.Now };
            _customerRepositoryMock.Setup(x => x.UpdateAsync(customer)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(customer);

            _customerRepositoryMock.Verify(x => x.UpdateAsync(customer), Times.Once);
        }
    }
}