using FluentValidation;
using System.Net;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractCompleteValidator: AbstractValidator<EntriesContractComplete>
    {
        public EntriesContractCompleteValidator()
        {
            #region EntryId

            RuleFor(x => x.EntryId).NotNull()
                                   .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The entry ID can`t be null."));

            RuleFor(x => x.EntryId).NotEmpty()
                                   .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The entry ID can`t be empty."));

            #endregion

            #region Completed

            RuleFor(x => x.Completed).NotNull()
                                     .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The entry completion can`t be null."));

            #endregion
        }
    }
}
