using System.Net;
using System.Text.Json;
using CrudByNgAndDotNet.API.Models;

namespace CrudByNgAndDotNet.API.Helper
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env; // Injected environment to track Development vs Production mode safely
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Capture structural logs containing detailed exception breakdown inside server logs
                _logger.LogError(ex, "An unhandled exception occurred during request execution: {Message}", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string clientMessage;
            object errorDetails;

            // Check the current hosting runtime environment dynamically
            if (_env.IsDevelopment())
            {
                // Local Development: Provide clear messages and full stack traces for easy debugging
                clientMessage = exception.Message;
                errorDetails = new { Details = exception.StackTrace };
            }
            else
            {
                // Live Production: Obfuscate raw system errors to protect infrastructure against exploits
                clientMessage = "A generic internal server error occurred. Please contact the technical support team.";
                errorDetails = "Internal telemetry tracking enabled. Logs captured securely on the host.";
            }

            // Using ApiResponse<object> dynamically handles both string logs and structured debug details flawlessly
            var apiResponse = new ApiResponse<object>
            {
                Status = false,
                StatusCode = context.Response.StatusCode,
                Message = clientMessage,
                Data = null,
                ErrorDetails = errorDetails
            };

            // Enforce unified camelCase json formatting configuration to prevent client-side Angular serialization breaks
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsJsonAsync(apiResponse, jsonOptions);
        }
    }
}
