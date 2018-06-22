using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PostgreSQL
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");
            using (var context = new MyDbContext())
            {
                await context.Database.EnsureDeletedAsync();
                await Task.Delay(TimeSpan.FromSeconds(3));
                await context.Database.EnsureCreatedAsync();

                for (var i = 1; i < 100; i++)
                {
                    var pet = new Pet
                    {
                        Category = new Category { Name = "Cats" },
                        Images =
                        {
                            new Image { Url = $"http://example.com/images/fluffy{i}_1.png" },
                            new Image { Url = $"http://example.com/images/fluffy{i}_2.png" },
                        },
                        Name = $"fluffy{i}",
                        Tags =
                        {
                            new Tag { Name = $"orange{i}" },
                            new Tag { Name = $"kitty{i}" },
                        },
                    };

                    context.Pets.Add(pet);
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch
                    {
                        Console.WriteLine($"Failed in attempt {i}.");
                        throw;
                    }
                }

                Console.WriteLine("Done");
            }
        }

        private class MyDbContext : DbContext
        {
            public DbSet<Category> Categories { get; set; }

            public DbSet<Image> Images { get; set; }

            public DbSet<Pet> Pets { get; set; }

            public DbSet<Tag> Tags { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
#if InMemory
                optionsBuilder.UseInMemoryDatabase(databaseName: "Joe");
#elif LocalDb
                optionsBuilder.UseSqlServer(connectionString: "Server=(localdb)\\mssqllocaldb;Database=Joe;" +
                    "Trusted_Connection=True;MultipleActiveResultSets=true");
#elif Sqlite
                optionsBuilder.UseSqlite(connectionString: "Data Source=Joe.db");
#elif SqlServer
                optionsBuilder.UseSqlServer(connectionString: "Server=GarnetAv5;Database=Joe;Trusted_Connection=True");
#else
                var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                optionsBuilder.UseNpgsql(connectionString);
#endif
            }
        }

        private class Category
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        private class Image
        {
            public int Id { get; set; }

            public string Url { get; set; }
        }

        private class Pet
        {
            public int Id { get; set; }

            public Category Category { get; set; }

            public string Name { get; set; }

            public List<Image> Images { get; set; } = new List<Image>();

            public List<Tag> Tags { get; set; } = new List<Tag>();
        }

        private class Tag
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
