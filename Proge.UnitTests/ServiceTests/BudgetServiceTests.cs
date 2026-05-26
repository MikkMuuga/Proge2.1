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
using Proge2._1.Services.Interfaces;

namespace Proge.UnitTests.ServiceTests
{
    public class BudgetServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBudgetRepository> _budgetRepositoryMock;
        private readonly BudgetService _service;

        public BudgetServiceTests()
        {
            _budgetRepositoryMock = new Mock<IBudgetRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.BudgetRepository).Returns(_budgetRepositoryMock.Object);
            _service = new BudgetService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAllBudgetsAsync_should_return_all_budgets()
        {
            var budgets = new List<Budget>
            {
                new Budget { Id = 1, Client = "Client 1", Date = DateTime.Now },
                new Budget { Id = 2, Client = "Client 2", Date = DateTime.Now }
            };
            _budgetRepositoryMock
                .Setup(x => x.GetBudgetsAsync())
                .ReturnsAsync(budgets);

            var result = await _service.GetAllBudgetsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetBudgetByIdAsync_should_return_budget()
        {
            var budget = new Budget { Id = 1, Client = "Client 1", Date = DateTime.Now };
            _budgetRepositoryMock
                .Setup(x => x.GetBudgetByIdAsync(1))
                .ReturnsAsync(budget);

            var result = await _service.GetBudgetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(budget, result);
        }

        [Fact]
        public async Task GetBudgetByIdAsync_should_return_null_when_not_found()
        {
            _budgetRepositoryMock
                .Setup(x => x.GetBudgetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Budget?)null);

            var result = await _service.GetBudgetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddBudgetAsync_should_call_repository_and_save()
        {
            var budget = new Budget { Id = 1, Client = "Client 1", Date = DateTime.Now };
            _budgetRepositoryMock.Setup(x => x.AddBudgetAsync(budget)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.AddBudgetAsync(budget);

            _budgetRepositoryMock.Verify(x => x.AddBudgetAsync(budget), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateBudgetAsync_should_call_repository_and_save()
        {
            var budget = new Budget { Id = 1, Client = "Client 1", Date = DateTime.Now };
            _budgetRepositoryMock.Setup(x => x.UpdateBudgetAsync(budget)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.UpdateBudgetAsync(budget);

            _budgetRepositoryMock.Verify(x => x.UpdateBudgetAsync(budget), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteBudgetAsync_should_call_repository_and_save()
        {
            _budgetRepositoryMock.Setup(x => x.DeleteBudgetAsync(1)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.DeleteBudgetAsync(1);

            _budgetRepositoryMock.Verify(x => x.DeleteBudgetAsync(1), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public void CalculateTotalCost_should_return_correct_value()
        {
            var budget = new Budget { Id = 1, Client = "Client 1", Date = DateTime.Now, ServiceCost = 100m };

            var result = _service.CalculateTotalCost(budget);

            Assert.Equal(120m, result);
        }

        [Fact]
        public async Task List_should_return_paged_results()
        {
            var budgets = new List<Budget>
            {
                new Budget { Id = 1, Client = "Client 1", Date = DateTime.Now },
                new Budget { Id = 2, Client = "Client 2", Date = DateTime.Now }
            };
            _budgetRepositoryMock
                .Setup(x => x.GetBudgetsAsync())
                .ReturnsAsync(budgets);

            var result = await _service.List(1, 10, new BudgetSearch());

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
        }

        [Fact]
        public async Task List_should_filter_by_client()
        {
            var budgets = new List<Budget>
            {
                new Budget { Id = 1, Client = "Alice", Date = DateTime.Now },
                new Budget { Id = 2, Client = "Bob", Date = DateTime.Now }
            };
            _budgetRepositoryMock
                .Setup(x => x.GetBudgetsAsync())
                .ReturnsAsync(budgets);

            var search = new BudgetSearch { Client = "Alice" };

            var result = await _service.List(1, 10, search);

            Assert.Equal(1, result.TotalItems);
        }
        [Fact]
        public async Task Save_should_add_new_budget_when_id_is_zero()
        {
            var budget = new Budget { Id = 0, Client = "New Client", Date = DateTime.Now };
            _budgetRepositoryMock.Setup(x => x.AddBudgetAsync(budget)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(budget);

            _budgetRepositoryMock.Verify(x => x.AddBudgetAsync(budget), Times.Once);
        }

        [Fact]
        public async Task Save_should_update_existing_budget_when_id_is_not_zero()
        {
            var budget = new Budget { Id = 1, Client = "Existing Client", Date = DateTime.Now };
            _budgetRepositoryMock.Setup(x => x.UpdateBudgetAsync(budget)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(budget);

            _budgetRepositoryMock.Verify(x => x.UpdateBudgetAsync(budget), Times.Once);
        }
    }
}