using Proge2_1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Proge2._1.Data;

namespace Proge2_1.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {
            // Check if there's any data in the database already
            if (context.Customers.Any())
            {
                return;
            }

            // Add sample customers
            var customer1 = new Customer
            {
                Name = "John Smith",
                Date = DateTime.Now,
                Contact = "john.smith@example.com"
            };

            var customer2 = new Customer
            {
                Name = "Mary Johnson",
                Date = DateTime.Now,
                Contact = "+372 5555 1234"
            };

            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();
        }
    }
}