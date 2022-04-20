using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uzdevums.Models;
using Microsoft.EntityFrameworkCore;

namespace Uzdevums.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ChangeLog> ChangeLogs { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            if (Database.EnsureCreated())
            {
                Users.AddRange(
                    new User { Name = "admin", Password = "admin", IsAdmin = true },
                    new User { Name = "user", Password = "user", IsAdmin = false }
                );

                Products.AddRange(
                    new Product { Name = "HDD 1TB", NumberOfUnits = 55, PricePerUnit = 74.09M },
                    new Product { Name = "HDD SSD 512GB", NumberOfUnits = 102, PricePerUnit = 190.99M },
                    new Product { Name = "RAM DDR4 16GB", NumberOfUnits = 47, PricePerUnit = 80.32M }
                );


                this.SaveChanges();
            }

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<User>().HasData(
            //        new User { UserId = 1, Name = "admin", Password = "admin", IsAdmin = true },
            //        new User { UserId = 2, Name = "user", Password = "user", IsAdmin = false }
            //       );

            //builder.Entity<Product>().HasData(
            //        new Product { Id = 1, Name = "HDD 1TB", NumberOfUnits = 55, PricePerUnit = 74.09M },
            //        new Product { Id = 2, Name = "HDD SSD 512GB", NumberOfUnits = 102, PricePerUnit = 190.99M },
            //        new Product { Id = 3, Name = "RAM DDR4 16GB", NumberOfUnits = 47, PricePerUnit = 80.32M }
            //    );
            base.OnModelCreating(builder);
        }
    }
}
