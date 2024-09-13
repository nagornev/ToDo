using FluentValidation;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Infrastructure.Providers;

namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class IdentityContractValidateValidator : AbstractValidator<IdentityContractValidate>
    {
        public IdentityContractValidateValidator()
        {
            #region Cookies


            RuleFor(x => x.Cookies).NotNull()
                                   .WithState(x => Errors.IsNull("The cookies can not be null."));

            RuleFor(x => x.Cookies).NotEmpty()
                                   .WithState(x => Errors.IsInvalidArgument("The cookies can not be empty."));

            RuleFor(x => x.Cookies).Must(cookies => cookies!.Any(x=>x.Key == JwtTokenProviderDefaults.Cookies))
                                   .WithState(x => Errors.IsInvalidArgument("The token was not found in the \"sign\" key."));
            #endregion

            #region Permissions

            RuleFor(x => x.Permissions).NotNull()
                                       .WithState(x => Errors.IsNull("The permissions can not be null."));

            RuleFor(x => x.Permissions).NotEmpty()
                                       .WithState(x => Errors.IsInvalidArgument("The permissions can not be empty"));

            RuleForEach(x => x.Permissions).IsInEnum()
                                           .WithState((x, invalidPermission) => Errors.IsInvalidArgument($"The permissions contain the invalid permission ({invalidPermission})."));

            #endregion
        }
    }
}
