using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core.Entities;  

namespace OrderManagementSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
        public DbSet<Product> Products => Set<Product>();
        public DbSet<AppUser> Users => Set<AppUser>();

        public DbSet<Order> Orders => Set<Order>();
    }
}
