using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proge2._1.Data;
using Microsoft.Extensions.Logging;
using Proge2_1.Data;

namespace Proge2._1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            try
            {
                // Load and validate the connection string
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                // Configure Identity
                builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

                // Add MVC Controllers and Razor Pages
                builder.Services.AddControllersWithViews();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Startup configuration error: {ex.Message}");
                throw; // Rethrow to halt application startup if configuration fails
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); // Enforce HSTS for production environments
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Database seeding (only in Debug mode)
#if DEBUG
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    // Ensure database is created and migrated
                    context.Database.Migrate();
                    // Seed the database
                    SeedData.Generate(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
#endif

            app.Run();
        }
    }
}