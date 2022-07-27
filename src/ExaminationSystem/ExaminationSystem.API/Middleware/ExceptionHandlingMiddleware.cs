using System.Net;
using Newtonsoft.Json;

namespace ExaminationSystem.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                _ => (int)HttpStatusCode.InternalServerError
            };

            JsonConvert.SerializeObject(ex.Message);
            await context.Response.WriteAsync(ex.Message);

            _logger.LogError(ex, "Something went wrong)");
        }
    }
}