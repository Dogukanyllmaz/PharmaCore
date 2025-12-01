using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            object responseObj;

            if (ex is ValidationException validationException)
            {
                statusCode = 400;
                responseObj = new
                {
                    StatusCode = statusCode,
                    Message = validationException.Message,
                    Errors = validationException.Errors.Select(f => new
                    {
                        f.PropertyName,
                        f.ErrorMessage
                    })
                };
            }
            else
            {
                responseObj = new
                {
                    StatusCode = statusCode,
                    Message = "Internal Server Error",
                    Detail = ex.Message
                };
            }

            context.Response.StatusCode = statusCode;

            // 🔥 Serilog loglama
            Log.Error(ex, "ExceptionMiddleware caught an exception: {@Response}", responseObj);

            var json = JsonSerializer.Serialize(responseObj);
            return context.Response.WriteAsync(json);
        }

    }
}