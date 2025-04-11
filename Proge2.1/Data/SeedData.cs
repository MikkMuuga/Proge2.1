using Microsoft.EntityFrameworkCore;
using Proge2_1.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Proge2._1.Data;

namespace Proge2_1.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {
            Console.WriteLine("SeedData.Generate method started");

            try
            {
                // Check if there's any data in the database already
                if (context.Budgets.Any())
                {
                    Console.WriteLine("Database already has budget data - exiting seed method");
                    return;
                }

                Console.WriteLine("No data found in database - seeding budget data now");

                // Add sample budgets - 10 as required
                Console.WriteLine("Adding 10 sample budgets");
                var budgets = new List<Budget>
                {
                    new Budget
                    {
                        Client = "TechCorp Ltd",
                        Date = DateTime.Now.AddDays(-30),
                        ServiceCost = 1500.00m,
                        TotalCost = 1875.00m
                    },
                    new Budget
                    {
                        Client = "Global Industries",
                        Date = DateTime.Now.AddDays(-25),
                        ServiceCost = 2200.50m,
                        TotalCost = 2750.63m
                    },
                    new Budget
                    {
                        Client = "Innovate Solutions",
                        Date = DateTime.Now.AddDays(-20),
                        ServiceCost = 3450.75m,
                        TotalCost = 4313.44m
                    },
                    new Budget
                    {
                        Client = "Digital Dynamics",
                        Date = DateTime.Now.AddDays(-15),
                        ServiceCost = 1875.25m,
                        TotalCost = 2344.06m
                    },
                    new Budget
                    {
                        Client = "Future Systems",
                        Date = DateTime.Now.AddDays(-10),
                        ServiceCost = 4200.00m,
                        TotalCost = 5250.00m
                    },
                    new Budget
                    {
                        Client = "Smart Solutions",
                        Date = DateTime.Now.AddDays(-5),
                        ServiceCost = 1250.50m,
                        TotalCost = 1563.13m
                    },
                    new Budget
                    {
                        Client = "EcoTech Partners",
                        Date = DateTime.Now.AddDays(-3),
                        ServiceCost = 3620.75m,
                        TotalCost = 4525.94m
                    },
                    new Budget
                    {
                        Client = "Advanced Analytics",
                        Date = DateTime.Now.AddDays(-2),
                        ServiceCost = 2900.25m,
                        TotalCost = 3625.31m
                    },
                    new Budget
                    {
                        Client = "Creative Concepts",
                        Date = DateTime.Now.AddDays(-1),
                        ServiceCost = 1750.00m,
                        TotalCost = 2187.50m
                    },
                    new Budget
                    {
                        Client = "Strategic Services",
                        Date = DateTime.Now,
                        ServiceCost = 5100.80m,
                        TotalCost = 6376.00m
                    }
                };

                // Add all budgets to the database
                context.Budgets.AddRange(budgets);
                Console.WriteLine("Budgets added to context");

                // Save all the added data to the database
                Console.WriteLine("Saving all changes to database...");
                int changesCount = context.SaveChanges();
                Console.WriteLine($"Successfully saved {changesCount} records to database");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in SeedData.Generate: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                // Re-throw the exception so it can be caught by the calling code
                throw;
            }
        }
    }
}