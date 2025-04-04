using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Proge2._1.Data;

public static class SeedData
{
    public static void Generate(DbContext context)
    {
        // Check if the Materials table already has data
        if (!context.Set<Material>().Any())
        {
            var materials = new List<Material>
            {
                new Material { Unit = "Cement Bag", Price = 10.50m, Seller = "Mip" },
                new Material { Unit = "Steel Rod", Price = 25.30m, Seller = "Pim" },
                new Material { Unit = "Bricks (100 pcs)", Price = 15.40m, Seller = "Tim" },
                new Material { Unit = "Sand (1 ton)", Price = 50.50m, Seller = "John" }
            };


            context.Set<Material>().AddRange(materials);
            context.SaveChanges();
        }
    }
}
public class Material
{
    public int Id { get; set; }
    public string Unit { get; set; } 
    public decimal Price { get; set; } 
    public string Seller { get; set; } 
}

public class YourDbContext : DbContext
{
    public DbSet<Material> Materials { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionStringHere");
    }
}
class Program
{
    static void Main(string[] args)
    {

        using (var context = new YourDbContext())
        {
            SeedData.Generate(context);
        }
    }
}
