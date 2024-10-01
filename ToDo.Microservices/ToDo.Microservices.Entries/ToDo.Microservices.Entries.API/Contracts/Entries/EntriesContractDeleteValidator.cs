using FluentValidation;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractDeleteValidator : AbstractValidator<EntriesContractDelete>
    {
        public EntriesContractDeleteValidator()
        {
            #region Id

            RuleFor(x => x.EntryId).NotNull()
                                      .WithState(x => Errors.IsNull("The entry id can not be null."));

            RuleFor(x => x.EntryId).NotEmpty()
                                      .WithState(x => Errors.IsNull("The entry id can not be empty."));

            #endregion
        }
    }
}
