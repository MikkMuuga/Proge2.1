using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proge2._1.Controllers;
using Proge2._1.Models;
using System.Diagnostics;

namespace Proge.UnitTests.ControllerTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_Returns_ViewResult()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_Returns_ViewResult()
        {
            var controller = new HomeController();

            var result = controller.Privacy();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_Returns_ViewResult_With_ErrorViewModel()
        {
            var controller = new HomeController();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Error();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }
    }
}