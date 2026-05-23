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
    public class ServicesControllerTests
    {
        private readonly Mock<IServicessService> _servicesServiceMock;
        private readonly ServicesController _controller;

        public ServicesControllerTests()
        {
            _servicesServiceMock = new Mock<IServicessService>();
            _controller = new ServicesController(_servicesServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var pagedResult = new PagedResult<Servicess>
            {
                Results = new List<Servicess>
                {
                    new Servicess { ServiceId = 1, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" },
                    new Servicess { ServiceId = 2, transportation = "Transport 2", PanelProduction = 200m, montage = "Montage 2" }
                },
                TotalItems = 2
            };

            _servicesServiceMock
                .Setup(x => x.GetPagedServices(page, 10, It.IsAny<ServiceSearch>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ServiceIndexModel>(result.Model);
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
        public async Task Details_should_return_notfound_when_service_not_found()
        {
            // Arrange
            _servicesServiceMock
                .Setup(x => x.GetServiceById(It.IsAny<int>()))
                .ReturnsAsync((Servicess?)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_service()
        {
            // Arrange
            var service = new Servicess { ServiceId = 1, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };

            _servicesServiceMock
                .Setup(x => x.GetServiceById(1))
                .ReturnsAsync(service);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(service, result.Model);
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
        public async Task Edit_should_return_notfound_when_service_not_found()
        {
            // Arrange
            _servicesServiceMock
                .Setup(x => x.GetServiceById(It.IsAny<int>()))
                .ReturnsAsync((Servicess?)null);

            // Act
            var result = await _controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_service()
        {
            // Arrange
            var service = new Servicess { ServiceId = 1, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };

            _servicesServiceMock
                .Setup(x => x.GetServiceById(1))
                .ReturnsAsync(service);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(service, result.Model);
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
        public async Task Delete_should_return_view_with_service()
        {
            // Arrange
            var service = new Servicess { ServiceId = 1, transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };

            _servicesServiceMock
                .Setup(x => x.GetServiceById(1))
                .ReturnsAsync(service);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(service, result.Model);
        }
    }
}