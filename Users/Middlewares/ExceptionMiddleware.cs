using System.Net;
using System.Text.Json;

namespace Users.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> log)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var payload = new { message = "Unexpected server error" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}