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
    public class CustomersControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public CustomersControllerTests()
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
            using var response = await _client.GetAsync("/Customers");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Customers/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Customers/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_found()
        {
            var customer = new Customer { Name = "Test", Contact = "Contact 1", Date = DateTime.Now };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Customers/Details/" + customer.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Customers/Edit/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_should_return_ok_when_found()
        {
            var customer = new Customer { Name = "Test", Contact = "Contact 1", Date = DateTime.Now };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Customers/Edit/" + customer.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Customers/Delete/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_ok_when_found()
        {
            var customer = new Customer { Name = "Test", Contact = "Contact 1", Date = DateTime.Now };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Customers/Delete/" + customer.Id);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Create_should_save_new_customer()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Name", "Test Customer" },
                { "Contact", "Test Contact" },
                { "Date", "2026-01-01" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Customers/Create", content);

            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);

            var customer = _context.Customers.FirstOrDefault();
            Assert.NotNull(customer);
            Assert.Equal("Test Customer", customer.Name);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_customer()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Name", "" },
                { "Contact", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Customers/Create", content);

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Customers.Any());
        }
    }
}
