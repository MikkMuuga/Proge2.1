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
using Proge2._1.Services;

namespace Proge.UnitTests.ControllerTests
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _controller = new CustomersController(_customerServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var pagedResult = new PagedResult<Customer>
            {
                Results = new List<Customer>
                {
                    new Customer { Id = 1, Name = "Test 1", Contact = "Contact 1", Date = DateTime.Now },
                    new Customer { Id = 2, Name = "Test 2", Contact = "Contact 2", Date = DateTime.Now }
                },
                TotalItems = 2
            };

            _customerServiceMock
                .Setup(x => x.GetPagedCustomers(page, 10, It.IsAny<CustomerSearch>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<CustomerIndexModel>(result.Model);
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
        public async Task Details_should_return_notfound_when_customer_not_found()
        {
            // Arrange
            _customerServiceMock
                .Setup(x => x.GetCustomerById(It.IsAny<int>()))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_customer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test", Contact = "Contact 1", Date = DateTime.Now };

            _customerServiceMock
                .Setup(x => x.GetCustomerById(1))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer, result.Model);
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
        public async Task Edit_should_return_notfound_when_customer_not_found()
        {
            // Arrange
            _customerServiceMock
                .Setup(x => x.GetCustomerById(It.IsAny<int>()))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_customer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test", Contact = "Contact 1", Date = DateTime.Now };

            _customerServiceMock
                .Setup(x => x.GetCustomerById(1))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer, result.Model);
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
        public async Task Delete_should_return_view_with_customer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test", Contact = "Contact 1", Date = DateTime.Now };

            _customerServiceMock
                .Setup(x => x.GetCustomerById(1))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer, result.Model);
        }
    }
}