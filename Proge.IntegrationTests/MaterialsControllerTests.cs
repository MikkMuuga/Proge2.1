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
    public class MaterialsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public MaterialsControllerTests()
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
            using var response = await _client.GetAsync("/Materials");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Materials/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Materials/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_found()
        {
            var material = new Materials { Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _context.Materials.Add(material);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Materials/Details/" + material.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Materials/Edit/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_should_return_ok_when_found()
        {
            var material = new Materials { Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _context.Materials.Add(material);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Materials/Edit/" + material.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Materials/Delete/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_ok_when_found()
        {
            var material = new Materials { Unit = "kg", Price = 10.00m, Seller = "Seller 1" };
            _context.Materials.Add(material);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Materials/Delete/" + material.Id);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Create_should_save_new_material()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Unit", "kg" },
                { "Price", "10.00" },
                { "Seller", "Test Seller" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Materials/Create", content);

            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);

            var material = _context.Materials.FirstOrDefault();
            Assert.NotNull(material);
            Assert.Equal("kg", material.Unit);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_material()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Unit", "" },
                { "Price", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Materials/Create", content);

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Materials.Any());
        }
    }
}
