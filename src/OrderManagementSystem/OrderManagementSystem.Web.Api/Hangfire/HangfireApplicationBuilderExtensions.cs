using Hangfire;
using Hangfire.Dashboard;
using OrderManagementSystem.Infrastructure.Jobs;

namespace OrderManagementSystem.Web.Api.Hangfire;

public static class HangfireApplicationBuilderExtensions
{
    public static IApplicationBuilder UseHangfireSetup(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Dashboard (dev/prod stratejini burada yönet)
        if (env.IsDevelopment())
        {
            app.UseHangfireDashboard("/hangfire");
        }
        else
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            });
        }

        // Recurring job'lar (3. adım: schedule)
        RecurringJob.AddOrUpdate<ReportJobService>(
            "daily-report",
            x => x.DailyReport(),
            "0 1 * * *" // her gün 01:00
        );

        return app;
    }
}