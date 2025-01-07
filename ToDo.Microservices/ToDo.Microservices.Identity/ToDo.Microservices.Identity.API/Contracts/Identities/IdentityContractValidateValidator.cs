using FluentValidation;
using System.Net;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Errors;

namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class IdentityContractValidateValidator : AbstractValidator<IdentityContractValidate>
    {
        public IdentityContractValidateValidator()
        {
            //#region Cookies


            //RuleFor(x => x.Cookies).NotNull()
            //                       .WithState(x => Errors.IsNull("The cookies can not be null."));

            //RuleFor(x => x.Cookies).NotEmpty()
            //                       .WithState(x => Errors.IsInvalidArgument("The cookies can not be empty."));

            //RuleFor(x => x.Cookies).Must(cookies => cookies!.Any(x=>x.Key == JwtTokenProviderDefaults.Cookies))
            //                       .WithState(x => Errors.IsInvalidArgument("The token was not found in the \"sign\" key."));
            //#endregion

            #region Permissions

            RuleFor(x => x.Permissions).NotNull()
                                       .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The permissions can`t be null."));

            RuleFor(x => x.Permissions).NotEmpty()
                                       .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The permissions can`t be empty"));

            RuleForEach(x => x.Permissions).IsInEnum()
                                           .WithState((x, invalidPermission) => new DefaultError(HttpStatusCode.BadRequest, $"The permissions contain the invalid permission ({invalidPermission})."));

            #endregion
        }
    }
}
