using FluentValidation;
using System.Net;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Errors;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractUpdateValidator : AbstractValidator<EntriesContractUpdate>
    {
        public EntriesContractUpdateValidator()
        {
            #region Id

            RuleFor(x => x.EntryId).NotNull()
                                      .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The entry ID can`t be null."));

            RuleFor(x => x.EntryId).NotEmpty()
                                      .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The entry ID can`t be empty."));

            #endregion

            #region CategoryId

            RuleFor(x => x.CategoryId).NotNull()
                                      .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The category ID can`t be null."));

            RuleFor(x => x.CategoryId).NotEmpty()
                                      .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The category ID can`t be empty."));

            #endregion

            #region Text

            RuleFor(x => x.Text).NotNull()
                              .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The entry text can`t be null."));

            RuleFor(x => x.Text).NotEmpty()
                                .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The entry text can`t be empty."));


            RuleFor(x => x.Text).MaximumLength(Entry.TextMaximumLength)
                                .WithState(x => new DefaultError(HttpStatusCode.BadRequest, $"The entry text can`t be more than {Entry.TextMaximumLength} symbols."));

            #endregion
        }
    }
}
