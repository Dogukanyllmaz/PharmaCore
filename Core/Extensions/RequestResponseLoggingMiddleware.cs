using Microsoft.AspNetCore.Http;
using Serilog;

namespace Core.Extensions
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            string requestBody = "";
            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            Log.Information("HTTP {Method} {Url} responded {StatusCode}. RequestBody: {@RequestBody} ResponseBody: {@ResponseBody}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                requestBody,
                text);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
