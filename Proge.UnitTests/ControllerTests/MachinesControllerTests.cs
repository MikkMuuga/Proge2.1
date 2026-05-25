using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Proge2._1.Controllers;
using Proge2._1.Data;
using Proge2._1.Models;
using Proge2._1.Search;
using Proge2._1.Services.Interfaces;

namespace Proge.UnitTests.ControllerTests
{
    public class MachinesControllerTests
    {
        private readonly Mock<IMachineService> _machineServiceMock;
        private readonly MachinesController _controller;

        public MachinesControllerTests()
        {
            _machineServiceMock = new Mock<IMachineService>();
            _controller = new MachinesController(_machineServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var pagedResult = new PagedResult<Machines>
            {
                Results = new List<Machines>
                {
                    new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 },
                    new Machines { Id = 2, Workers = "Worker 2", Supervision = "Super 2", CostOfMachines = 200 }
                },
                TotalItems = 2
            };

            _machineServiceMock
                .Setup(x => x.GetPagedMachines(page, 10, It.IsAny<MachineSearch>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<MachineIndexModel>(result.Model);
            Assert.Equal(pagedResult, model.Data);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_null()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_machine_not_found()
        {
            // Arrange
            _machineServiceMock
                .Setup(x => x.GetMachineById(It.IsAny<int>()))
                .ReturnsAsync((Machines?)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_machine()
        {
            // Arrange
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };

            _machineServiceMock
                .Setup(x => x.GetMachineById(1))
                .ReturnsAsync(machine);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(machine, result.Model);
        }

        [Fact]
        public void Create_should_return_view()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_null()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_machine_not_found()
        {
            // Arrange
            _machineServiceMock
                .Setup(x => x.GetMachineById(It.IsAny<int>()))
                .ReturnsAsync((Machines?)null);

            // Act
            var result = await _controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_machine()
        {
            // Arrange
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };

            _machineServiceMock
                .Setup(x => x.GetMachineById(1))
                .ReturnsAsync(machine);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(machine, result.Model);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_machine()
        {
            // Arrange
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };

            _machineServiceMock
                .Setup(x => x.GetMachineById(1))
                .ReturnsAsync(machine);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(machine, result.Model);
        }
        [Fact]
        public async Task Create_post_should_return_view_when_modelstate_invalid()
        {
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _controller.ModelState.AddModelError("key", "error");

            var result = await _controller.Create(machine) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(machine, result.Model);
        }

        [Fact]
        public async Task Create_post_should_redirect_when_modelstate_valid()
        {
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _machineServiceMock.Setup(x => x.AddMachine(machine)).Verifiable();

            var result = await _controller.Create(machine) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _machineServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_post_should_return_notfound_when_id_mismatch()
        {
            var machine = new Machines { Id = 2, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };

            var result = await _controller.Edit(1, machine);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_post_should_return_view_when_modelstate_invalid()
        {
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _controller.ModelState.AddModelError("key", "error");

            var result = await _controller.Edit(1, machine) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(machine, result.Model);
        }

        [Fact]
        public async Task Edit_post_should_redirect_when_modelstate_valid()
        {
            var machine = new Machines { Id = 1, Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _machineServiceMock.Setup(x => x.UpdateMachine(machine)).Verifiable();

            var result = await _controller.Edit(1, machine) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _machineServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_and_redirect()
        {
            int id = 1;
            _machineServiceMock.Setup(x => x.DeleteMachine(id)).Verifiable();

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _machineServiceMock.VerifyAll();
        }
    }
}