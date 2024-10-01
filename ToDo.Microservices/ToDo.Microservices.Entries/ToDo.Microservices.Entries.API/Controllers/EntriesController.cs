using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ToDo.Domain.Results;
using ToDo.Extensions.Validator;
using ToDo.Microservices.Entries.API.Contracts.Entries;
using ToDo.Microservices.Entries.Domain.Collectings;
using ToDo.Microservices.Entries.UseCases.Services;
using ToDo.Microservices.Middleware.Identities;

namespace ToDo.Microservices.Entries.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntriesController : Controller
    {
        private IEntryService _entryService;

        public EntriesController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        [Identity(IdentityPermissions.User)]
        public async Task<IResult> Get()
        {
            Result<IEnumerable<EntryCompose>> resultEntries = await _entryService.GetEntries(User.GetId());

            return resultEntries.Success ?
                    Results.Ok(resultEntries) :
                    Results.BadRequest(resultEntries);
        }

        [HttpGet]
        [Identity(IdentityPermissions.User)]
        [Route("{id}")]
        public async Task<IResult> Get(Guid id)
        {
            Result<EntryCompose> resultEntry = await _entryService.GetEntry(User.GetId(),
                                                                            id);

            return resultEntry.Success ?
                    Results.Ok(resultEntry) :
                    Results.BadRequest(resultEntry);
        }

        [HttpPost]
        [Identity(IdentityPermissions.User)]
        public async Task<IResult> Create([FromServices] IValidator<EntriesContractCreate> validator,
                                          [FromBody] EntriesContractCreate contract)
        {
            if (!validator.Validate(contract, out Result validationResult))
                return Results.BadRequest(validationResult);

            Result creationResult = await _entryService.CreateEntry(User.GetId(),
                                                                    contract.CategoryId,
                                                                    contract.Text,
                                                                    contract.Deadline);

            return creationResult.Success ?
                    Results.Ok(creationResult) :
                    Results.BadRequest(creationResult);
        }

        [HttpPut]
        [Identity(IdentityPermissions.User)]
        public async Task<IResult> Update([FromServices] IValidator<EntriesContractUpdate> validator,
                                          [FromBody] EntriesContractUpdate contract)
        {
            if (!validator.Validate(contract, out Result validationResult))
                return Results.BadRequest(validationResult);

            Result updateResult = await _entryService.UpdateEntry(User.GetId(),
                                                                  contract.EntryId,
                                                                  contract.CategoryId,
                                                                  contract.Text,
                                                                  contract.Deadline,
                                                                  contract.Completed);

            return updateResult.Success ?
                    Results.Ok(updateResult) :
                    Results.BadRequest(updateResult);
        }

        [HttpDelete]
        [Identity(IdentityPermissions.User)]
        public async Task<IResult> Delete([FromServices] IValidator<EntriesContractDelete> validator,
                                          [FromBody] EntriesContractDelete contract)
        {
            if (!validator.Validate(contract, out Result validationResult))
                return Results.BadRequest(validationResult);

            Result deleteResult = await _entryService.DeleteEntry(User.GetId(),
                                                                  contract.EntryId);

            return deleteResult.Success ?
                    Results.Ok(deleteResult) :
                    Results.BadRequest(deleteResult);
        }
    }
}
