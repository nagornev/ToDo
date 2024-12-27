using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ToDo.Microservices.Middleware.Users
{
    public static class UserStartupExtensions
    {
        public static void AddUserValidator<TUserValidator>(this IServiceCollection services)
            where TUserValidator : class, IUserValidator
        {
            services.AddScoped<IUserValidator, TUserValidator>();
            services.AddScoped<IUserValidatorProvider, UserValidatorProvider>();
        }

        public static void UseUserValidator(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserMiddleware>();
        }
    }
}
