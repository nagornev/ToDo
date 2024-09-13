using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Results;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Entries.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntitiesController : Controller
    {
        [HttpGet]
        [Route("[action]")]
        [Identity(IdentityPermissions.User)]
        public async Task<IResult> GetUserEntries()
        {
            IEnumerable<string> entries = new string[]
            {
                "The first user entry in this to do list",
                "The second user entry",
            };

            return Results.Ok(Result<IEnumerable<string>>.Successful(entries));
        }

        [HttpGet]
        [Route("[action]")]
        [Identity(IdentityPermissions.Super)]
        public async Task<IResult> GetSuperEntries()
        {
            IEnumerable<string> entries = new string[]
            {
                "The first super entry in this to do list",
                "The second super entry",
            };

            return Results.Ok(Result<IEnumerable<string>>.Successful(entries));
        }
    }
}
