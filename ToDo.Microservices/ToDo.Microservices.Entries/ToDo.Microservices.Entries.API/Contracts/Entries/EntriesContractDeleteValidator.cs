using FluentValidation;
using System.Net;
using ToDo.Domain.Results.Errors;

namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractDeleteValidator : AbstractValidator<EntriesContractDelete>
    {
        public EntriesContractDeleteValidator()
        {
            #region Id

            RuleFor(x => x.EntryId).NotNull()
                                   .NotEmpty()
                                      .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The entry ID can`t be null."));

            RuleFor(x => x.EntryId).NotEmpty()
                                      .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The entry ID can`t be empty."));

            #endregion
        }
    }
}
