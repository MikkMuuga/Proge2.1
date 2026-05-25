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
    public class CommentsControllerTests
    {
        private readonly Mock<ICommentService> _commentServiceMock;
        private readonly CommentsController _controller;

        public CommentsControllerTests()
        {
            _commentServiceMock = new Mock<ICommentService>();
            _controller = new CommentsController(_commentServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var pagedResult = new PagedResult<Comment>
            {
                Items = new List<Comment>
                {
                    new Comment { Id = 1, Content = "Test 1", User = "User 1" },
                    new Comment { Id = 2, Content = "Test 2", User = "User 2" }
                },
                TotalItems = 2
            };

            _commentServiceMock
                .Setup(x => x.GetPagedComments(page, 10, It.IsAny<CommentSearch>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<CommentIndexModel>(result.Model);
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
        public async Task Details_should_return_notfound_when_comment_not_found()
        {
            // Arrange
            _commentServiceMock
                .Setup(x => x.GetCommentById(It.IsAny<int>()))
                .ReturnsAsync((Comment?)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_comment()
        {
            // Arrange
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };

            _commentServiceMock
                .Setup(x => x.GetCommentById(1))
                .ReturnsAsync(comment);

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(comment, result.Model);
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
        public async Task Edit_should_return_notfound_when_comment_not_found()
        {
            // Arrange
            _commentServiceMock
                .Setup(x => x.GetCommentById(It.IsAny<int>()))
                .ReturnsAsync((Comment?)null);

            // Act
            var result = await _controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_comment()
        {
            // Arrange
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };

            _commentServiceMock
                .Setup(x => x.GetCommentById(1))
                .ReturnsAsync(comment);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(comment, result.Model);
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
        public async Task Delete_should_return_view_with_comment()
        {
            // Arrange
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };

            _commentServiceMock
                .Setup(x => x.GetCommentById(1))
                .ReturnsAsync(comment);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(comment, result.Model);
        }
        [Fact]
        public async Task Create_post_should_return_view_when_modelstate_invalid()
        {
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };
            _controller.ModelState.AddModelError("key", "error");

            var result = await _controller.Create(comment) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(comment, result.Model);
        }

        [Fact]
        public async Task Create_post_should_redirect_when_modelstate_valid()
        {
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };
            _commentServiceMock.Setup(x => x.AddComment(comment)).Verifiable();

            var result = await _controller.Create(comment) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _commentServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_post_should_return_notfound_when_id_mismatch()
        {
            var comment = new Comment { Id = 2, Content = "Test", User = "User 1" };

            var result = await _controller.Edit(1, comment);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_post_should_return_view_when_modelstate_invalid()
        {
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };
            _controller.ModelState.AddModelError("key", "error");

            var result = await _controller.Edit(1, comment) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(comment, result.Model);
        }

        [Fact]
        public async Task Edit_post_should_redirect_when_modelstate_valid()
        {
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };
            _commentServiceMock.Setup(x => x.UpdateComment(comment)).Verifiable();

            var result = await _controller.Edit(1, comment) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _commentServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_and_redirect()
        {
            int id = 1;
            _commentServiceMock.Setup(x => x.DeleteComment(id)).Verifiable();

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _commentServiceMock.VerifyAll();
        }
    }
}