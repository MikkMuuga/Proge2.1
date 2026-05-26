using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Search;
using Proge2._1.Services;
using Proge2._1.Services.Interfaces;

namespace Proge.UnitTests.ServiceTests
{
    public class ServicesServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IServicessRepository> _servicesRepositoryMock;
        private readonly ApplicationDbContext _context;
        private readonly ServicesService _service;

        public ServicesServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _servicesRepositoryMock = new Mock<IServicessRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.ServicesRepository).Returns(_servicesRepositoryMock.Object);
            _service = new ServicesService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetServiceById_should_return_service()
        {
            var service = new Servicess { ServiceId = 1, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };
            _servicesRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(service);

            var result = await _service.GetServiceById(1);

            Assert.NotNull(result);
            Assert.Equal(service, result);
        }

        [Fact]
        public async Task GetServiceById_should_return_null_when_not_found()
        {
            _servicesRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Servicess?)null);

            var result = await _service.GetServiceById(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddService_should_call_repository_and_save()
        {
            var service = new Servicess { ServiceId = 1, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };
            _servicesRepositoryMock.Setup(x => x.AddAsync(service)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.AddService(service);

            _servicesRepositoryMock.Verify(x => x.AddAsync(service), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateService_should_call_repository_and_save()
        {
            var service = new Servicess { ServiceId = 1, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };
            _servicesRepositoryMock.Setup(x => x.UpdateAsync(service)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.UpdateService(service);

            _servicesRepositoryMock.Verify(x => x.UpdateAsync(service), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteService_should_call_repository_and_save()
        {
            _servicesRepositoryMock.Setup(x => x.DeleteAsync(1)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.DeleteService(1);

            _servicesRepositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ServiceExists_should_return_true_when_exists()
        {
            _servicesRepositoryMock
                .Setup(x => x.ExistsAsync(1))
                .ReturnsAsync(true);

            var result = await _service.ServiceExists(1);

            Assert.True(result);
        }

        [Fact]
        public async Task ServiceExists_should_return_false_when_not_found()
        {
            _servicesRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _service.ServiceExists(99);

            Assert.False(result);
        }

        [Fact]
        public async Task GetPagedServices_should_return_paged_results()
        {
            var pagedResult = new PagedResult<Servicess>
            {
                Results = new List<Servicess>
        {
            new Servicess { ServiceId = 1, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" },
            new Servicess { ServiceId = 2, transportation = "Transport 2", PanelProduction = 200m, montage = "Montage 2" }
        },
                TotalItems = 2
            };

            _servicesRepositoryMock
                .Setup(x => x.GetPagedAsync(1, 10))
                .ReturnsAsync(pagedResult);

            var result = await _service.GetPagedServices(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
        }
        [Fact]
        public async Task Save_should_add_new_service_when_id_is_zero()
        {
            var service = new Servicess { ServiceId = 0, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };
            _servicesRepositoryMock.Setup(x => x.AddAsync(service)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(service);

            _servicesRepositoryMock.Verify(x => x.AddAsync(service), Times.Once);
        }

        [Fact]
        public async Task Save_should_update_existing_service_when_id_is_not_zero()
        {
            var service = new Servicess { ServiceId = 1, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };
            _servicesRepositoryMock.Setup(x => x.UpdateAsync(service)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(service);

            _servicesRepositoryMock.Verify(x => x.UpdateAsync(service), Times.Once);
        }
    }
}