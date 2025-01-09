using FluentValidation;
using System.Net;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.API.Contracts.Categories
{
    public class CategoriesContractUpdateValidator : AbstractValidator<CategoriesContractUpdate>
    {
        public CategoriesContractUpdateValidator()
        {
            #region Id

            RuleFor(x => x.CategoryId).NotNull()
                                      .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The category id can`t be null."));

            RuleFor(x => x.CategoryId).NotEmpty()
                                      .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The category id can`t be empty."));

            #endregion

            #region Name

            RuleFor(x => x.Name).NotNull()
                                .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The category name can`t be null."));

            RuleFor(x => x.Name).NotEmpty()
                                .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.NullOrEmpty, "The category name can`t be empty."));

            RuleFor(x => x.Name).MaximumLength(Category.NameMaximumLength)
                                .WithState(x => new Error(HttpStatusCode.BadRequest, ErrorKeys.InvalidArgument, $"The category name can`t be more than {Category.NameMaximumLength} symbols."));

            #endregion
        }
    }
}
