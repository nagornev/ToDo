using FluentValidation;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class IdentityContractValidateValidator : AbstractValidator<IdentityContractValidate>
    {
        //JWT token pattern
        private const string _tokenPattern = "(^[A-Za-z0-9-_]*\\.[A-Za-z0-9-_]*\\.[A-Za-z0-9-_]*$)";

        public IdentityContractValidateValidator()
        {
            #region Token

            RuleFor(x => x.Token).Matches(_tokenPattern)
                                 .WithState(x => Errors.IsInvalidArgument("No JWT token. The token has an invalid format."));

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
