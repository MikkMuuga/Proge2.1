using Microsoft.AspNetCore.Mvc;
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
using Xunit.Abstractions;

namespace Proge.IntegrationTests
{
    [Collection("Sequential")]
    public class BudgetsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        private readonly ITestOutputHelper _output;

        public BudgetsControllerTests(ITestOutputHelper output)
        {
            _output = output;
            Console.SetOut(new ConsoleWriter(output));
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            };
            _client = Factory.CreateClient(options);
            _context = Factory.Services.GetService<ApplicationDbContext>();
        }

        public class ConsoleWriter : StringWriter
        {
            private readonly ITestOutputHelper _output;
            public ConsoleWriter(ITestOutputHelper output) => _output = output;
            public override void WriteLine(string value)
            {
                try { _output.WriteLine(value); }
                catch {}
            }
        }

        [Fact]
        public async Task Index_should_return_success()
        {
            using var response = await _client.GetAsync("/Budgets");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Budgets/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Budgets/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_found()
        {
            var budget = new Budget { Client = "Test", Date = DateTime.Now, ServiceCost = 100, TotalCost = 120 };
            _context.Budgets.Add(budget);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Budgets/Details/" + budget.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Budgets/Edit/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_should_return_ok_when_found()
        {
            var budget = new Budget { Client = "Test", Date = DateTime.Now, ServiceCost = 100, TotalCost = 120 };
            _context.Budgets.Add(budget);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Budgets/Edit/" + budget.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Budgets/Delete/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_ok_when_found()
        {
            var budget = new Budget { Client = "Test", Date = DateTime.Now, ServiceCost = 100, TotalCost = 120 };
            _context.Budgets.Add(budget);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Budgets/Delete/" + budget.Id);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Create_should_save_new_budget()
        {
            var formValues = new Dictionary<string, string>
    {
        { "Client", "Test Client" },
        { "Date", "02/01/2026" },
        { "ServiceCost", "100.00" },
        { "TotalCost", "120.00" },
    };
            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Budgets/Create", content);

            var body = await response.Content.ReadAsStringAsync();
            _output.WriteLine("STATUS: " + response.StatusCode);
            _output.WriteLine("BODY: " + body);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            var savedBudget = _context.Budgets.FirstOrDefault(b => b.Client == "Test Client");
            Assert.NotNull(savedBudget);
        }


        [Fact]
        public async Task Create_should_not_save_invalid_budget()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Date", "2026-02-01" },
                { "ServiceCost", "100.00" },
                { "TotalCost", "120.00" },
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Budgets/Create", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var savedBudget = _context.Budgets.FirstOrDefault(b => b.Client == "Test Client");
            Assert.Null(savedBudget);
        }
    }
}
