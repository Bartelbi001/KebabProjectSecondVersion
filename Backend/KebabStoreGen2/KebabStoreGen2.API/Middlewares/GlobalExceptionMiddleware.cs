using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace KebabStoreGen2.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    public static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        Log.Error(exception, "Unhandled exception occurred.");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new { message = "An unexpected error occured", detailt = exception.Message };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}