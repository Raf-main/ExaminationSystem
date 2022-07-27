using ExaminationSystem.API.Middleware;

namespace ExaminationSystem.API.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void UseExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}