using Hangfire;
using Hangfire.SqlServer;
using OrderManagementSystem.Infrastructure.Jobs;

namespace OrderManagementSystem.Web.Api.Hangfire;

public static class HangfireServiceCollectionExtensions
{
    public static IServiceCollection AddHangfireSetup(this IServiceCollection services, IConfiguration config)
    {
        var connStr = config.GetConnectionString("DefaultConnection")
                     ?? throw new InvalidOperationException("DefaultConnection is missing.");

        services.AddHangfire(hf =>
        {
            hf.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
              .UseSimpleAssemblyNameTypeSerializer()
              .UseRecommendedSerializerSettings()
              .UseSqlServerStorage(connStr, new SqlServerStorageOptions
              {
                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                  QueuePollInterval = TimeSpan.FromSeconds(10),
                  UseRecommendedIsolationLevel = true,
                  DisableGlobalLocks = true
              });
        });

        services.AddHangfireServer();

        // Job service'leri (1. adımda yazdıkların)
        services.AddScoped<ReportJobService>();
        // services.AddScoped<InventoryJobService>(); // yeni job ekleyince buraya

        return services;
    }
}