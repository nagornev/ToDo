using FluentValidation;
using System.Net;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Errors;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.API.Contracts.Categories
{
    public class CategoriesContractCreateValidator : AbstractValidator<CategoriesContractCreate>
    {
        public CategoriesContractCreateValidator()
        {
            #region Name

            RuleFor(x => x.Name).NotNull()
                                .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The category name can`t be null."));

            RuleFor(x => x.Name).NotEmpty()
                                .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The category name can`t be empty."));

            RuleFor(x => x.Name).MaximumLength(Category.NameMaximumLength)
                                .WithState(x => new DefaultError(HttpStatusCode.BadRequest, $"The category name can`t be more than {Category.NameMaximumLength} symbols."));

            #endregion
        }
    }
}
