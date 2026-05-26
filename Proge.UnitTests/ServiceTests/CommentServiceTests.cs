using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Search;
using Proge2._1.Services;
using Microsoft.EntityFrameworkCore.InMemory;

namespace Proge.UnitTests.ServiceTests
{
    public class CommentServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly ApplicationDbContext _context;
        private readonly CommentService _service;

        public CommentServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.CommentRepository).Returns(_commentRepositoryMock.Object);
            _service = new CommentService(_unitOfWorkMock.Object, _context);
        }

        private (CommentService service, ApplicationDbContext context) CreateServiceWithContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            var service = new CommentService(_unitOfWorkMock.Object, context);
            return (service, context);
        }

        [Fact]
        public async Task GetCommentById_should_return_comment()
        {
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };
            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(comment);

            var result = await _service.GetCommentById(1);

            Assert.NotNull(result);
            Assert.Equal(comment, result);
        }

        [Fact]
        public async Task GetCommentById_should_return_null_when_not_found()
        {
            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Comment?)null);

            var result = await _service.GetCommentById(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddComment_should_call_repository_and_save()
        {
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };
            _commentRepositoryMock.Setup(x => x.AddAsync(comment)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.AddComment(comment);

            _commentRepositoryMock.Verify(x => x.AddAsync(comment), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateComment_should_call_repository_and_save()
        {
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };
            _commentRepositoryMock.Setup(x => x.UpdateAsync(comment)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.UpdateComment(comment);

            _commentRepositoryMock.Verify(x => x.UpdateAsync(comment), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteComment_should_call_repository_and_save()
        {
            _commentRepositoryMock.Setup(x => x.DeleteAsync(1)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.DeleteComment(1);

            _commentRepositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CommentExists_should_return_true_when_exists()
        {
            var comment = new Comment { Id = 1, Content = "Test", User = "User 1" };
            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(comment);

            var result = await _service.CommentExists(1);

            Assert.True(result);
        }

        [Fact]
        public async Task CommentExists_should_return_false_when_not_found()
        {
            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Comment?)null);

            var result = await _service.CommentExists(99);

            Assert.False(result);
        }

        [Fact]
        public async Task GetPagedComments_should_return_paged_results()
        {
            var (service, context) = CreateServiceWithContext();

            context.Comments.AddRange(
                new Comment { Id = 1, Content = "Test 1", User = "User 1" },
                new Comment { Id = 2, Content = "Test 2", User = "User 2" }
            );
            await context.SaveChangesAsync();

            var result = await service.GetPagedComments(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.Results.Count);
        }

        [Fact]
        public async Task GetPagedComments_should_filter_by_content()
        {
            var (service, context) = CreateServiceWithContext();

            context.Comments.AddRange(
                new Comment { Id = 1, Content = "Hello", User = "User 1" },
                new Comment { Id = 2, Content = "World", User = "User 2" }
            );
            await context.SaveChangesAsync();

            var result = await service.GetPagedComments(1, 10, new CommentSearch { Content = "Hello" });

            Assert.Equal(1, result.Results.Count);
        }
        [Fact]
        public async Task Save_should_add_new_comment_when_id_is_zero()
        {
            var comment = new Comment { Id = 0, Content = "New Comment", User = "User 1" };
            _commentRepositoryMock.Setup(x => x.AddAsync(comment)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(comment);

            _commentRepositoryMock.Verify(x => x.AddAsync(comment), Times.Once);
        }

        [Fact]
        public async Task Save_should_update_existing_comment_when_id_is_not_zero()
        {
            var comment = new Comment { Id = 1, Content = "Existing Comment", User = "User 1" };
            _commentRepositoryMock.Setup(x => x.UpdateAsync(comment)).Verifiable();
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            await _service.Save(comment);

            _commentRepositoryMock.Verify(x => x.UpdateAsync(comment), Times.Once);
        }
    }
}