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
    public class MachineServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMachinesRepository> _machinesRepositoryMock;
        private readonly MachineService _service;

        public MachineServiceTests()
        {
            _machinesRepositoryMock = new Mock<IMachinesRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.MachinesRepository).Returns(_machinesRepositoryMock.Object);
            _service = new MachineService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetMachineById_should_return_machine()
        {
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _machinesRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(machine);

            var result = await _service.GetMachineById(1);

            Assert.NotNull(result);
            Assert.Equal(machine, result);
        }

        [Fact]
        public async Task GetMachineById_should_return_null_when_not_found()
        {
            _machinesRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Machines?)null);

            var result = await _service.GetMachineById(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddMachine_should_call_repository_and_save()
        {
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _machinesRepositoryMock.Setup(x => x.AddAsync(machine)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.AddMachine(machine);

            _machinesRepositoryMock.Verify(x => x.AddAsync(machine), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateMachine_should_call_repository_and_save()
        {
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _machinesRepositoryMock.Setup(x => x.UpdateAsync(machine)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.UpdateMachine(machine);

            _machinesRepositoryMock.Verify(x => x.UpdateAsync(machine), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteMachine_should_call_repository_and_save()
        {
            _machinesRepositoryMock.Setup(x => x.DeleteAsync(1)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.DeleteMachine(1);

            _machinesRepositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task MachineExists_should_return_true_when_exists()
        {
            _machinesRepositoryMock
                .Setup(x => x.ExistsAsync(1))
                .ReturnsAsync(true);

            var result = await _service.MachineExists(1);

            Assert.True(result);
        }

        [Fact]
        public async Task MachineExists_should_return_false_when_not_found()
        {
            _machinesRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _service.MachineExists(99);

            Assert.False(result);
        }

        [Fact]
        public async Task GetPagedMachines_should_return_paged_results()
        {
            var pagedResult = new PagedResult<Machines>
            {
                Results = new List<Machines>
                {
                    new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 },
                    new Machines { Id = 2, Workers = "Worker 2", Supervision = "Super 2", CostOfMachines = 200 }
                },
                TotalItems = 2
            };
            _machinesRepositoryMock
                .Setup(x => x.GetPagedAsync(1, 10))
                .ReturnsAsync(pagedResult);

            var result = await _service.GetPagedMachines(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
        }
        [Fact]
        public async Task Save_should_add_new_machine_when_id_is_zero()
        {
            var machine = new Machines { Id = 0, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _machinesRepositoryMock.Setup(x => x.AddAsync(machine)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(machine);

            _machinesRepositoryMock.Verify(x => x.AddAsync(machine), Times.Once);
        }

        [Fact]
        public async Task Save_should_update_existing_machine_when_id_is_not_zero()
        {
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _machinesRepositoryMock.Setup(x => x.UpdateAsync(machine)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(machine);

            _machinesRepositoryMock.Verify(x => x.UpdateAsync(machine), Times.Once);
        }
    }
}