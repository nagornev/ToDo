using FluentValidation;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractUpdateValidator : AbstractValidator<EntriesContractUpdate>
    {
        public EntriesContractUpdateValidator()
        {
            #region Id

            RuleFor(x => x.EntryId).NotNull()
                                      .WithState(x => Errors.IsNull("The entry id can not be null."));

            RuleFor(x => x.EntryId).NotEmpty()
                                      .WithState(x => Errors.IsNull("The entry id can not be empty."));

            #endregion

            #region CategoryId

            RuleFor(x => x.CategoryId).NotNull()
                                      .WithState(x => Errors.IsNull("The category id can not be null."));

            RuleFor(x => x.CategoryId).NotEmpty()
                                      .WithState(x => Errors.IsNull("The category id can not be empty."));

            #endregion

            #region Text

            RuleFor(x => x.Text).NotNull()
                              .WithState(x => Errors.IsNull("The entry text can not be null."));

            RuleFor(x => x.Text).NotEmpty()
                                .WithState(x => Errors.IsNull("The entry text can not be empty."));


            RuleFor(x => x.Text).MaximumLength(Entry.TextMaximumLength)
                                .WithState(x => Errors.IsNull("The entry text can not be longer than 100 symbols."));

            #endregion

            #region Datetime

            RuleFor(x => x.Deadline).Must(x =>
                                    {
                                        if (x != null)
                                        {
                                            if (x < DateTime.Now)
                                                return false;

                                            return true;
                                        }

                                        return true;
                                    })
                                    .WithState(x => Errors.IsNull("The entry text can not be longer than 100 symbols."));

            #endregion
        }
    }
}
