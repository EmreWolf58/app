using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Infrastructure.Jobs
{
    public class ReportJobService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ReportJobService> _logger;

        public ReportJobService(AppDbContext db, ILogger<ReportJobService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task DailyReport()
        {
            var today = DateTime.UtcNow.Date;
            var count = await _db.Orders.CountAsync();

            _logger.LogInformation("DailyReport {Date}: Total orders in DB = {Count}", today, count);
        }
    }
}
