using FluentValidation;
using System.Net;
using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class IdentityContractSignInValidator : AbstractValidator<IdentityContractSignIn>
    {
        public IdentityContractSignInValidator()
        {
            #region Email

            RuleFor(x => x.Email).NotNull()
                                 .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The user email can`t be null."));

            RuleFor(x => x.Email).NotEmpty()
                                 .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The user email can`t be empty."));

            RuleFor(x => x.Email).EmailAddress()
                                 .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.InvalidArgument, "The user email has an invalid format."));

            #endregion

            #region Password

            RuleFor(x => x.Password).NotNull()
                                    .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The user password can`t be null."));

            RuleFor(x => x.Password).NotEmpty()
                                    .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The user password can`t be empty."));

            RuleFor(x => x.Password).Matches(User.PasswordExpression)
                                    .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.InvalidArgument, "The user password has an invalid format."));

            #endregion
        }
    }
}
