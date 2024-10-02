using Microsoft.AspNetCore.Builder;

namespace ToDo.Microservices.Middleware.Exceptions
{
    public static class GlobalExceptionHandlerStartupExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
