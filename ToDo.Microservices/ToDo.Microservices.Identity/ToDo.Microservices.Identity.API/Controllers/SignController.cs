using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Results;
using ToDo.Extensions;
using ToDo.Microservices.Identity.API.Contracts.Sign;
using ToDo.Microservices.Identity.UseCases.Services;

namespace ToDo.Microservices.Identity.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignController : Controller
    {
        private IUserService _userService;

        public SignController(IUserService userSerice)
        {
            _userService = userSerice;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IResult> Up([FromServices] IValidator<SignContractUp> validator,
                                      [FromBody] SignContractUp contract)
        {
            if (!validator.Validate(contract, out Result validationResult))
                return Results.BadRequest(validationResult);

            Result resultUp = await _userService.SignUp(contract.Email,
                                                       contract.Password);

            return resultUp.Success ?
                    Results.Ok() :
                    Results.BadRequest(resultUp);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IResult> In([FromServices] IValidator<SignContractIn> validator,
                                      [FromBody] SignContractIn contract)
        {
            if (!validator.Validate(contract, out Result validationResult))
                return Results.BadRequest(validationResult);

            Result<string> resultIn = await _userService.SignIn(contract.Email,
                                                                contract.Password);

            return resultIn.Success ?
                    Results.Ok(resultIn) :
                    Results.BadRequest(resultIn);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IResult> Accessable([FromServices] IValidator<SignContractAccess> validator,
                                              [FromBody] SignContractAccess contract)
        {
            if (!validator.Validate(contract, out Result validationResult))
                return Results.BadRequest(validationResult);

            Result resultAccess = await _userService.IsAccessable(contract.Token,
                                                                  contract.Permissions);

            return resultAccess.Success ?
                    Results.Ok(resultAccess) :
                    Results.BadRequest(resultAccess);
        }
    }
}
