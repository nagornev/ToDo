using FluentValidation;
using ToDo.Microservices.Entries.API.Contracts.Entries;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class ValidatorsStartupExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<EntriesContractCreate>, EntriesContractCreateValidator>();
            services.AddScoped<IValidator<EntriesContractUpdate>, EntriesContractUpdateValidator>();
            services.AddScoped<IValidator<EntriesContractDelete>, EntriesContractDeleteValidator>();
        }
    }
}
