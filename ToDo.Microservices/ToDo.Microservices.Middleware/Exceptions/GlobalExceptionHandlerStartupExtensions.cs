using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ToDo.Microservices.Middleware.Exceptions
{
    public static class GlobalExceptionHandlerStartupExtensions
    {
        public static void AddGlobalExceptionHandlerConfiguration(this IServiceCollection services, Action<GlobalExceptionHandlerConfigurationBuilder> options)
        {
            GlobalExceptionHandlerConfigurationBuilder builder = new GlobalExceptionHandlerConfigurationBuilder();

            options.Invoke(builder);

            services.AddSingleton<IGlobalExceptionHandlerConfiguration>(builder.Build());
        }

        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
