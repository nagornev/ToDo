using FluentValidation;
using System.Net;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Categories.API.Contracts.Categories
{
    public class CategoriesContractDeleteValidator : AbstractValidator<CategoriesContractDelete>
    {
        public CategoriesContractDeleteValidator()
        {
            #region Id

            RuleFor(x => x.CategoryId).NotNull()
                                      .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The category id can`t be null."));

            RuleFor(x => x.CategoryId).NotEmpty()
                                      .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The category id can`t be empty."));

            #endregion
        }
    }
}
