using FluentValidation;
using System.Net;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Errors;

namespace ToDo.Microservices.Categories.API.Contracts.Categories
{
    public class CategoriesContractDeleteValidator : AbstractValidator<CategoriesContractDelete>
    {
        public CategoriesContractDeleteValidator()
        {
            #region Id

            RuleFor(x => x.CategoryId).NotNull()
                                      .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The category id can`t be null."));

            RuleFor(x => x.CategoryId).NotEmpty()
                                      .WithState(x => new DefaultError(HttpStatusCode.BadRequest, "The category id can`t be empty."));

            #endregion
        }
    }
}
