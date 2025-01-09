using FluentValidation;
using System.Net;
using ToDo.Domain.Results;

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
                                       .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The permissions can`t be null."));

            RuleFor(x => x.Permissions).NotEmpty()
                                       .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The permissions can`t be empty"));

            RuleForEach(x => x.Permissions).IsInEnum()
                                           .WithState((x, invalidPermission) => new Error(HttpStatusCode.BadRequest, ErrorKeys.InvalidArgument, $"The permissions contain the invalid permission ({invalidPermission})."));

            #endregion
        }
    }
}
