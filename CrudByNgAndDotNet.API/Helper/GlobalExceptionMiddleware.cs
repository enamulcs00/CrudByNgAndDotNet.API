
using CrudByNgAndDotNet.API.Models;

namespace CrudByNgAndDotNet.API.Helper

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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new ApiResponse<string>
            {
                Status = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = exception.Message,
                ErrorDetails = new { exception.StackTrace }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}

