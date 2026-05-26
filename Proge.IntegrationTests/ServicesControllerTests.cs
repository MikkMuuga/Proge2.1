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
    public class ServicesControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public ServicesControllerTests()
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
            using var response = await _client.GetAsync("/Services");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Services/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Services/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_found()
        {
            var service = new Servicess { transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };
            _context.Services.Add(service);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Services/Details/" + service.ServiceId);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Services/Edit/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_should_return_ok_when_found()
        {
            var service = new Servicess { transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };
            _context.Services.Add(service);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Services/Edit/" + service.ServiceId);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Services/Delete/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_ok_when_found()
        {
            var service = new Servicess { transportation = "Transport 1", PanelProduction = 100m, montage = "Montage 1" };
            _context.Services.Add(service);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Services/Delete/" + service.ServiceId);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Create_should_save_new_service()
        {
            var formValues = new Dictionary<string, string>
            {
                { "ServiceId", "0" },
                { "transportation", "Test Transport" },
                { "PanelProduction", "100" },
                { "montage", "Test Montage" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Services/Create", content);

            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);

            var service = _context.Services.FirstOrDefault();
            Assert.NotNull(service);
            Assert.Equal("Test Transport", service.transportation);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_service()
        {
            var formValues = new Dictionary<string, string>
            {
                { "transportation", "" },
                { "montage", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Services/Create", content);

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Services.Any());
        }
    }
}
