using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Proge2._1.Controllers;
using Proge2._1.Data;
using Proge2._1.Models;
using Proge2._1.Search;
using Proge2._1.Services.Interfaces;

namespace Proge.UnitTests.ControllerTests
{
    public class BudgetsControllerTests
    {
        private readonly Mock<IBudgetService> _budgetServiceMock;
        private readonly BudgetsController _controller;

        public BudgetsControllerTests()
        {
            _budgetServiceMock = new Mock<IBudgetService>();
            _controller = new BudgetsController(_budgetServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            int size = 10;
            var search = new BudgetSearch();
            var data = new List<Budget>
            {
                new Budget { Id = 1, Client = "Test Client 1", Date = DateTime.Now },
                new Budget { Id = 2, Client = "Test Client 2", Date = DateTime.Now }
            };
            var pagedResult = new PagedResult<Budget> { Items = data, TotalItems = 2 };

            _budgetServiceMock
                .Setup(x => x.ListAsync(page, size, It.IsAny<BudgetSearch>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page, size, search) as ViewResult;

            // Assert
            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<BudgetIndexModel>(result.Model);
            Assert.Equal(pagedResult.Results, model.Budgets);
            Assert.Equal(2, model.TotalItems);
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
        public async Task Details_should_return_notfound_when_budget_not_found()
        {
            // Arrange
            _budgetServiceMock
                .Setup(x => x.GetBudgetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Budget?)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_budget()
        {
            // Arrange
            var budget = new Budget { Id = 1, Client = "Test Budget", Date = DateTime.Now };

            _budgetServiceMock
                .Setup(x => x.GetBudgetByIdAsync(1))
                .ReturnsAsync(budget);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(budget, result.Model);
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
        public async Task Edit_should_return_notfound_when_budget_not_found()
        {
            // Arrange
            _budgetServiceMock
                .Setup(x => x.GetBudgetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Budget?)null);

            // Act
            var result = await _controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_budget()
        {
            // Arrange
            var budget = new Budget { Id = 1, Client = "Test Budget", Date = DateTime.Now };

            _budgetServiceMock
                .Setup(x => x.GetBudgetByIdAsync(1))
                .ReturnsAsync(budget);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(budget, result.Model);
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
        public async Task Delete_should_return_view_with_budget()
        {
            // Arrange
            var budget = new Budget { Id = 1, Client = "Test Budget", Date = DateTime.Now };

            _budgetServiceMock
                .Setup(x => x.GetBudgetByIdAsync(1))
                .ReturnsAsync(budget);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(budget, result.Model);
        }
    }
}