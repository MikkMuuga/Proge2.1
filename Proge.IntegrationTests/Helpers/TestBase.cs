using System;
using Microsoft.Extensions.DependencyInjection;
using Proge2._1.Data;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Proge.IntegrationTests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        public WebApplicationFactory<FakeStartup> Factory { get; }

        public TestBase()
        {
            Factory = new TestApplicationFactory<FakeStartup>();
        }

        public void Dispose()
        {
            var dbContext = Factory.Services.GetService<ApplicationDbContext>();
            dbContext?.Database.EnsureDeleted();
        }
    }
}
