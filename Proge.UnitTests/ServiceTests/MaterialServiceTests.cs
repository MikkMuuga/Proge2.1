using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Search;
using Proge2._1.Services;

namespace Proge.UnitTests.ServiceTests
{
    public class MaterialServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMaterialsRepository> _materialsRepositoryMock;
        private readonly MaterialService _service;

        public MaterialServiceTests()
        {
            _materialsRepositoryMock = new Mock<IMaterialsRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.MaterialsRepository).Returns(_materialsRepositoryMock.Object);
            _service = new MaterialService(_unitOfWorkMock.Object);
        }

        private (MaterialService service, ApplicationDbContext context) CreateServiceWithContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            var service = new MaterialService(_unitOfWorkMock.Object);
            return (service, context);
        }

        [Fact]
        public async Task GetMaterialById_should_return_material()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _materialsRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(material);

            var result = await _service.GetMaterialById(1);

            Assert.NotNull(result);
            Assert.Equal(material, result);
        }

        [Fact]
        public async Task GetMaterialById_should_return_null_when_not_found()
        {
            _materialsRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Materials?)null);

            var result = await _service.GetMaterialById(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddMaterial_should_call_repository_and_save()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.AddMaterial(material);

            _materialsRepositoryMock.Verify(x => x.AddAsync(material), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task AddMaterial_should_rollback_on_exception()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _materialsRepositoryMock.Setup(x => x.AddAsync(material)).ThrowsAsync(new Exception("DB error"));
            _unitOfWorkMock.Setup(x => x.RollbackAsync()).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<Exception>(() => _service.AddMaterial(material));

            _unitOfWorkMock.Verify(x => x.RollbackAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateMaterial_should_call_repository_and_save()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.UpdateMaterial(material);

            _materialsRepositoryMock.Verify(x => x.UpdateAsync(material), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateMaterial_should_rollback_on_exception()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _materialsRepositoryMock.Setup(x => x.UpdateAsync(material)).ThrowsAsync(new Exception("DB error"));
            _unitOfWorkMock.Setup(x => x.RollbackAsync()).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateMaterial(material));

            _unitOfWorkMock.Verify(x => x.RollbackAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteMaterial_should_call_repository_and_save()
        {
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.DeleteMaterial(1);

            _materialsRepositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteMaterial_should_rollback_on_exception()
        {
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _materialsRepositoryMock.Setup(x => x.DeleteAsync(1)).ThrowsAsync(new Exception("DB error"));
            _unitOfWorkMock.Setup(x => x.RollbackAsync()).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteMaterial(1));

            _unitOfWorkMock.Verify(x => x.RollbackAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task MaterialExists_should_return_true_when_exists()
        {
            _materialsRepositoryMock.Setup(x => x.ExistsAsync(1)).ReturnsAsync(true);

            var result = await _service.MaterialExists(1);

            Assert.True(result);
        }

        [Fact]
        public async Task MaterialExists_should_return_false_when_not_found()
        {
            _materialsRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _service.MaterialExists(99);

            Assert.False(result);
        }

        [Fact]
        public async Task GetPagedMaterials_should_return_paged_results()
        {
            var pagedResult = new PagedResult<Materials>
            {
                Results = new List<Materials>
                {
                    new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" },
                    new Materials { Id = 2, Unit = "m",  Price = 20.00m, Seller = "Seller 2" }
                },
                TotalItems = 2
            };
            _materialsRepositoryMock.Setup(x => x.GetPagedAsync(1, 10)).ReturnsAsync(pagedResult);

            var result = await _service.GetPagedMaterials(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(2, result.Results.Count);
        }

        [Fact]
        public async Task GetPagedMaterials_should_filter_by_unit()
        {
            var pagedResult = new PagedResult<Materials>
            {
                Results = new List<Materials>
                {
                    new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" }
                },
                TotalCount = 1
            };
            _materialsRepositoryMock
                .Setup(x => x.GetPagedAsync(1, 10, It.IsAny<MaterialSearch>()))
                .ReturnsAsync(pagedResult);

            var result = await _service.GetPagedMaterials(1, 10, new MaterialSearch { Unit = "kg" });

            Assert.Equal(1, result.TotalCount);
            Assert.Equal("kg", result.Results[0].Unit);
        }

        [Fact]
        public async Task GetPagedMaterials_should_filter_by_seller()
        {
            var pagedResult = new PagedResult<Materials>
            {
                Results = new List<Materials>
                {
                    new Materials { Id = 2, Unit = "m", Price = 20.00m, Seller = "Seller 2" }
                },
                TotalCount = 1
            };
            _materialsRepositoryMock
                .Setup(x => x.GetPagedAsync(1, 10, It.IsAny<MaterialSearch>()))
                .ReturnsAsync(pagedResult);

            var result = await _service.GetPagedMaterials(1, 10, new MaterialSearch { Seller = "Seller 2" });

            Assert.Equal(1, result.TotalCount);
            Assert.Equal("Seller 2", result.Results[0].Seller);
        }
        [Fact]
        public async Task Save_should_add_new_material_when_id_is_zero()
        {
            var material = new Materials { Id = 0, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _materialsRepositoryMock.Setup(x => x.AddAsync(material)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(material);

            _materialsRepositoryMock.Verify(x => x.AddAsync(material), Times.Once);
        }

        [Fact]
        public async Task Save_should_update_existing_material_when_id_is_not_zero()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _materialsRepositoryMock.Setup(x => x.UpdateAsync(material)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(material);

            _materialsRepositoryMock.Verify(x => x.UpdateAsync(material), Times.Once);
        }
    }
}