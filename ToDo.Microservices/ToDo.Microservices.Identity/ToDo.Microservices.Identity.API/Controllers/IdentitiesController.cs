using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Results;
using ToDo.Extensions.Validator;
using ToDo.Microservices.Identity.API.Contracts.Sign;
using ToDo.Microservices.Identity.Infrastructure.Providers;
using ToDo.Microservices.Identity.UseCases.Services;
using ToDo.Microservices.Identity.Infrastructure.Extensions;

namespace ToDo.Microservices.Identity.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentitiesController : Controller
    {
        private IUserService _userService;

        public IdentitiesController(IUserService userSerice)
        {
            _userService = userSerice;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IResult> SignUp([FromServices] IValidator<IdentityContractSignUp> validator,
                                          [FromBody] IdentityContractSignUp contract)
        {
            if (!validator.Validate(contract, out Result validationResult))
                return Results.BadRequest(validationResult);

            Result upResult = await _userService.SignUp(contract.Email,
                                                        contract.Password);



            return upResult.Success ?
                    Results.Ok(upResult) :
                    Results.BadRequest(upResult);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IResult> SignIn([FromServices] IValidator<IdentityContractSignIn> validator,
                                          [FromBody] IdentityContractSignIn contract)
        {
            if (!validator.Validate(contract, out Result validationResult))
                return Results.BadRequest(validationResult);

            Result<string> inResult = await _userService.SignIn(contract.Email,
                                                                contract.Password,
                                                                (token) => HttpContext.Response.Cookies.Append(JwtTokenProviderDefaults.Cookies, token));

            return inResult.Success ?
                    Results.Ok(inResult) :
                    Results.BadRequest(inResult);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IResult> Validate([FromServices] IValidator<IdentityContractValidate> validator,
                                            [FromBody] IdentityContractValidate contract)
        {
            if (!validator.Validate(contract, out Result validationResult) ||
                !HttpContext.Request.Cookies.TryGetValue(JwtTokenProviderDefaults.Cookies, out string? token))
                return !validationResult.Success ?
                        Results.BadRequest(validationResult) :
                        Results.BadRequest(Result.Failure(Errors.IsNull("No cookies.")));

            Result<Guid?> accessResult = await _userService.Validate(token,
                                                                     contract.Permissions);

            return accessResult.Success ?
                    Results.Ok(accessResult) :
                    Results.BadRequest(accessResult);
        }
    }
}
