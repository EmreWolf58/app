using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            const int maxAttempts = 15;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await db.Database.MigrateAsync();
                    break; // başarılıysa döngüden çık
                }
                catch (Exception ex) when (
                    ex is SqlException ||
                    ex.InnerException is SqlException ||
                    ex.Message.Contains("A network-related", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("Cannot open database", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("Login failed", StringComparison.OrdinalIgnoreCase))
                {
                    if (attempt == maxAttempts)
                        throw;

                    await Task.Delay(TimeSpan.FromSeconds(2 * attempt));
                }
            }

            //User Seed
            if (!await db.Users.AnyAsync())
            {
                var hasher = new PasswordHasher<AppUser>();

                var admin = new AppUser { Username = "admin", Role = "Admin" };
                admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

                var user = new AppUser { Username = "user", Role = "User" };
                user.PasswordHash = hasher.HashPassword(user, "User123!");

                db.Users.AddRange(admin, user);
                await db.SaveChangesAsync();
            }

            // Orders seed
            if (!await db.Orders.AnyAsync())
            {
                db.Orders.AddRange(
                    new Order { OrderNo = "ORD-1001", CustomerName = "Ali Veli", TotalAmount = 1250, Status = "New" },
                    new Order { OrderNo = "ORD-1002", CustomerName = "Ayşe Yılmaz", TotalAmount = 499, Status = "Paid" }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}
