﻿using System.Net;
using Learning_Web.API.Exceptions;
using Learning_Web.API.Models.Response;

namespace Learning_Web.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        //fields
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;

        //constructor
        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var errorId = Guid.NewGuid().ToString();
                LogError(errorId, exception);
                await HandleExceptionAsync(context, errorId, exception);
            }
        }

        private void LogError(string errorId, Exception exception)
        {
            var error = new
            {
                ErrorId = errorId,
                ExceptionType = exception.GetType().Name,
                ExceptionMessage = exception.Message,
                StackTrace = exception.StackTrace
            };

            _logger.LogError(exception, "{@error}", error);
        }

        private async Task HandleExceptionAsync(HttpContext context, string errorId, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var message = GetMessage(exception);
            var reason = _environment.IsDevelopment() ? exception.StackTrace : "Internal Server Error";

            var response = new ApiResponse<object>
            {
                StatusCode = (int)statusCode,
                Message = message,
                Reason = reason,
                IsSuccess = false,
                Data = new
                {
                    ErrorId = errorId,
                    Timestamp = DateTime.UtcNow
                }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(response);
        }

        private static string GetMessage(Exception exception)
        {
            return exception switch
            {
                ApiException apiException => apiException.Message,
                UnauthorizedAccessException => "Unauthorized access",
                KeyNotFoundException => "Resource not found",
                ArgumentException => "Invalid argument provided",
                InvalidOperationException => "Invalid operation",
                _ => "An unexpected error occurred"
            };
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ApiException apiException => apiException.StatusCode,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                KeyNotFoundException => HttpStatusCode.NotFound,
                ArgumentException => HttpStatusCode.BadRequest,
                InvalidOperationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}
