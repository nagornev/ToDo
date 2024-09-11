using FluentValidation;
using ToDo.Microservices.Identity.API.Contracts.Sign;

namespace ToDo.Microservices.Identity.API.Extensions.Stratup
{
    public static class ValidatorsStartupExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<IdentityContractSignUp>, IdentityContractSignUpValidator>();
            services.AddScoped<IValidator<IdentityContractSignIn>, IdentityContractSignInValidator>();
            services.AddScoped<IValidator<IdentityContractValidate>, IdentityContractValidateValidator>();
        }
    }
}
