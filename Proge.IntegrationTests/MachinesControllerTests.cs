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
    public class MachinesControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public MachinesControllerTests()
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
            using var response = await _client.GetAsync("/Machines");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Machines/Details/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            using var response = await _client.GetAsync("/Machines/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_found()
        {
            var machine = new Machines { Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _context.Machines.Add(machine);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Machines/Details/" + machine.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Machines/Edit/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_should_return_ok_when_found()
        {
            var machine = new Machines { Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _context.Machines.Add(machine);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Machines/Edit/" + machine.Id);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_not_found()
        {
            using var response = await _client.GetAsync("/Machines/Delete/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_ok_when_found()
        {
            var machine = new Machines { Workers = "Worker 1", Supervision = "Super 1", CostOfMachines = 100 };
            _context.Machines.Add(machine);
            _context.SaveChanges();

            using var response = await _client.GetAsync("/Machines/Delete/" + machine.Id);
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Create_should_save_new_machine()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Workers", "Test Workers" },
                { "Supervision", "Test Supervision" },
                { "CostOfMachines", "100" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Machines/Create", content);

            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);

            var machine = _context.Machines.FirstOrDefault();
            Assert.NotNull(machine);
            Assert.Equal("Test Workers", machine.Workers);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_machine()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Workers", "" }
            };

            using var content = new FormUrlEncodedContent(formValues);
            using var response = await _client.PostAsync("/Machines/Create", content);

            response.EnsureSuccessStatusCode();
            Assert.False(_context.Machines.Any());
        }
    }
}
