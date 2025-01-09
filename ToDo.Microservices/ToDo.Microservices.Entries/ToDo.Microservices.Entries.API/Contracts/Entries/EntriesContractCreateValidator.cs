using FluentValidation;
using System.Net;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.API.Contracts.Entries
{
    public class EntriesContractCreateValidator : AbstractValidator<EntriesContractCreate>
    {
        public EntriesContractCreateValidator()
        {
            #region CategoryId

            RuleFor(x => x.CategoryId).NotNull()
                                      .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The category ID can`t be null."));

            RuleFor(x => x.CategoryId).NotEmpty()
                                      .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The category ID can`t be empty."));

            #endregion

            #region Text

            RuleFor(x => x.Text).NotNull()
                              .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The entry text can`t be null."));

            RuleFor(x => x.Text).NotEmpty()
                                .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The entry text can`t be empty."));


            RuleFor(x => x.Text).MaximumLength(Entry.TextMaximumLength)
                                .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.InvalidArgument, $"The entry text can`t be more than {Entry.TextMaximumLength} symbols."));

            #endregion
        }
    }
}
