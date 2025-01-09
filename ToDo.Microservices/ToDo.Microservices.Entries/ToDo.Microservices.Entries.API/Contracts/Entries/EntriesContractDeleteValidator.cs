using FluentValidation;
using System.Net;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractDeleteValidator : AbstractValidator<EntriesContractDelete>
    {
        public EntriesContractDeleteValidator()
        {
            #region Id

            RuleFor(x => x.EntryId).NotNull()
                                   .NotEmpty()
                                      .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The entry ID can`t be null."));

            RuleFor(x => x.EntryId).NotEmpty()
                                      .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The entry ID can`t be empty."));

            #endregion
        }
    }
}
