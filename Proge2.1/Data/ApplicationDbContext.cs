﻿using Proge2._1.Controllers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Proge2._1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Materials> Materials { get; set; }
        public DbSet<Servicess> Services { get; set; }
        public DbSet<Machines> Machines { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
