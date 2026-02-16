using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core.Dtos;

namespace OrderManagementSystem.Web.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke (HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                var traceId = context.TraceIdentifier;

                // Burada exception’ı logluyoruz (ex stack trace dahil)
                _logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", traceId);

                var (statusCode, message) = MapException(ex);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var payload = new ErrorResponse
                {
                    TraceId = traceId,
                    Message = message,
                    Status = statusCode
                };

                var json = JsonSerializer.Serialize(payload);
                await context.Response.WriteAsync(json);
            }
        }

        private static (int statusCode, string message) MapException (Exception ex)
        {
            //zamanla genişletilebilir.

            return ex switch
            {
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Unauthorized."),
                ArgumentException => ((int)HttpStatusCode.BadRequest, ex.Message),

                // DB hatalarını genelde 500 ya da 409 yapmak isteyebilirsin:
                DbUpdateException => ((int)HttpStatusCode.InternalServerError, "Database update error."),
                SqlException => ((int)HttpStatusCode.InternalServerError, "Database error."),

                _ => ((int)HttpStatusCode.InternalServerError, "Unexpected error occurred.")
            };
        }


    }
}
