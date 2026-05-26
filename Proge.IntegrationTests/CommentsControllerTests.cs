using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Proge.IntegrationTests.Helpers;
using Proge2._1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Xunit;

namespace Proge.IntegrationTests
{
    [Collection("Sequential")]
    public class CommentsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public CommentsControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _client = Factory.CreateClient(options);
            _context = Factory.Services.GetService<ApplicationDbContext>();
        }

        [Fact]
        public async Task Index_should_return_success()
        {
            using var response = await _client.GetAsync("/Comments");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Comments/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Comments/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_found()
        {
            var comment = new Comment { Content = "Test", User = "User 1" };
            _context.Comments.Add(comment);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Comments/Details/" + comment.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Comments/Edit/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_should_return_ok_when_found()
        {
            var comment = new Comment { Content = "Test", User = "User 1" };
            _context.Comments.Add(comment);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Comments/Edit/" + comment.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Comments/Delete/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_ok_when_found()
        {
            var comment = new Comment { Content = "Test", User = "User 1" };
            _context.Comments.Add(comment);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Comments/Delete/" + comment.Id);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Create_should_save_new_comment()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Content", "Test Content" },
                { "User", "Test User" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Comments/Create", content);

            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);

            var comment = _context.Comments.FirstOrDefault();
            Assert.NotNull(comment);
            Assert.Equal("Test Content", comment.Content);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_comment()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Content", "" },
                { "User", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Comments/Create", content);

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Comments.Any());
        }
    }
}
