using FluentValidation;
using ToDo.Microservices.Categories.API.Contracts.Categories;

namespace ToDo.Microservices.Categories.API.Extensions.Startup
{
    public static class ValidatorsStartupExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CategoriesContractCreate>, CategoriesContractCreateValidator>();

            services.AddScoped<IValidator<CategoriesContractUpdate>, CategoriesContractUpdateValidator>();

            services.AddScoped<IValidator<CategoriesContractDelete>, CategoriesContractDeleteValidator>();
        }
    }
}
