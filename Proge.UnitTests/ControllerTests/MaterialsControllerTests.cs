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
    public class MaterialsControllerTests
    {
        private readonly Mock<IMaterialService> _materialServiceMock;
        private readonly MaterialsController _controller;

        public MaterialsControllerTests()
        {
            _materialServiceMock = new Mock<IMaterialService>();
            _controller = new MaterialsController(_materialServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var pagedResult = new PagedResult<Materials>
            {
                Results = new List<Materials>
                {
                    new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" },
                    new Materials { Id = 2, Unit = "m", Price = 20.00m, Seller = "Seller 2" }
                },
                TotalItems = 2
            };

            _materialServiceMock
                .Setup(x => x.GetPagedMaterials(page, 10, It.IsAny<MaterialSearch>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<MaterialIndexModel>(result.Model);
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
        public async Task Details_should_return_notfound_when_material_not_found()
        {
            // Arrange
            _materialServiceMock
                .Setup(x => x.GetMaterialById(It.IsAny<int>()))
                .ReturnsAsync((Materials?)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_material()
        {
            // Arrange
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };

            _materialServiceMock
                .Setup(x => x.GetMaterialById(1))
                .ReturnsAsync(material);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(material, result.Model);
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
        public async Task Edit_should_return_notfound_when_material_not_found()
        {
            // Arrange
            _materialServiceMock
                .Setup(x => x.GetMaterialById(It.IsAny<int>()))
                .ReturnsAsync((Materials?)null);

            // Act
            var result = await _controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_material()
        {
            // Arrange
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };

            _materialServiceMock
                .Setup(x => x.GetMaterialById(1))
                .ReturnsAsync(material);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(material, result.Model);
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
        public async Task Delete_should_return_view_with_material()
        {
            // Arrange
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };

            _materialServiceMock
                .Setup(x => x.GetMaterialById(1))
                .ReturnsAsync(material);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(material, result.Model);
        }
        [Fact]
        public async Task Create_post_should_return_view_when_modelstate_invalid()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _controller.ModelState.AddModelError("key", "error");

            var result = await _controller.Create(material) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(material, result.Model);
        }

        [Fact]
        public async Task Create_post_should_redirect_when_modelstate_valid()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _materialServiceMock.Setup(x => x.AddMaterial(material)).Verifiable();

            var result = await _controller.Create(material) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _materialServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_post_should_return_notfound_when_id_mismatch()
        {
            var material = new Materials { Id = 2, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };

            var result = await _controller.Edit(1, material);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_post_should_return_view_when_modelstate_invalid()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _controller.ModelState.AddModelError("key", "error");

            var result = await _controller.Edit(1, material) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(material, result.Model);
        }

        [Fact]
        public async Task Edit_post_should_redirect_when_modelstate_valid()
        {
            var material = new Materials { Id = 1, Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _materialServiceMock.Setup(x => x.UpdateMaterial(material)).Verifiable();

            var result = await _controller.Edit(1, material) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _materialServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_and_redirect()
        {
            int id = 1;
            _materialServiceMock.Setup(x => x.DeleteMaterial(id)).Verifiable();

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _materialServiceMock.VerifyAll();
        }
    }
}