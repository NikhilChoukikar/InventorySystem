using System.Text.Json;
using FluentValidation;
using InventorySystem.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InventorySystem.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Application layer validation exception (contains pre-built Errors dictionary)
            if (exception is FluentValidation.ValidationException appValidation)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var payload = new { errors = appValidation.Errors };
                return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }

            // FluentValidation exception (map ValidationFailure -> dictionary)
            if (exception is FluentValidation.ValidationException fvEx)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = fvEx.Errors?
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(g => g.Key, g => g.ToArray())
                    ?? new Dictionary<string, string[]>();

                var payload = new { errors };
                return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }

            // Unhandled exception: log and return generic 500
            _logger.LogError(exception, "Unhandled exception occurred while processing request.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
            return context.Response.WriteAsync(result);
        }
    }
}