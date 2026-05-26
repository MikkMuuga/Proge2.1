using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proge2._1.Controllers;
using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Services;
using Proge2._1.Services.Interfaces;

namespace Proge.IntegrationTests.Helpers
{
    public class FakeStartup
    {
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Microsoft.AspNetCore.Antiforgery.IAntiforgery, FakeAntiforgery>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("SharedTestDb")
                       .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IMachineService, MachineService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IServicessService, ServicesService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IMaterialsRepository, MaterialsRepository>();
            services.AddScoped<IServicessRepository, ServicessRepository>();
            services.AddScoped<IMachinesRepository, MachinesRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AllowAnonymousFilter());
            }).AddApplicationPart(typeof(HomeController).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

        public class FakeAntiforgery : Microsoft.AspNetCore.Antiforgery.IAntiforgery
        {
        public Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext)
            => new Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet("test", "test", "test", "test");

        public Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet GetTokens(HttpContext httpContext)
            => new Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet("test", "test", "test", "test");

        public Task<bool> IsRequestValidAsync(HttpContext httpContext)
            => Task.FromResult(true);

        public void SetCookieTokenAndHeader(HttpContext httpContext) { }

        public Task ValidateRequestAsync(HttpContext httpContext)
            => Task.CompletedTask;
        }
}