using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OrderManagementSystem.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Migration çalışırken config’i buradan okuyacağız
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            // Paket Manager Console genelde startup projesinin klasöründen çalışır
            // Bu yüzden appsettings.json'un doğru path’ini vermek gerekebilir.
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("src/OrderManagementSystem.Web.Api/appsettings.json", optional: true)
            .AddJsonFile("OrderManagementSystem.Web.Api/appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connStr = configuration.GetConnectionString("DefaultConnection")
                     ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection bulunamadı.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connStr);

        return new AppDbContext(optionsBuilder.Options);
    }
}