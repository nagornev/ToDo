using FluentValidation;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class SignContractInValidator : AbstractValidator<SignContractIn>
    {
        public SignContractInValidator()
        {
            #region Email

            RuleFor(x => x.Email).NotNull()
                                 .WithState(x => Errors.IsNull("The user email can not be null.", nameof(x.Email)));

            RuleFor(x => x.Email).NotEmpty()
                                 .WithState(x => Errors.IsNull("The user email can not be empty.", nameof(x.Email)));

            RuleFor(x => x.Email).EmailAddress()
                                 .WithState(x => Errors.IsInvalidArgument("The user email has an invalid format.", nameof(x.Email)));

            #endregion


            #region Password

            RuleFor(x => x.Password).NotNull()
                                    .WithState(x => Errors.IsNull("The user password can not be null.", nameof(x.Password)));

            RuleFor(x => x.Password).NotEmpty()
                                    .WithState(x => Errors.IsNull("The user password can not be empty.", nameof(x.Password)));

            RuleFor(x => x.Password).Matches(User.PasswordPattern)
                                    .WithState(x => Errors.IsInvalidArgument("The user password has an invalid format.", nameof(x.Password)));

            #endregion
        }
    }
}
