using FluentValidation;
using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;

namespace ToDo.Microservices.Categories.API.Contracts.Categories
{
    public class CategoriesContractUpdateValidator:AbstractValidator<CategoriesContractUpdate>
    {
        public CategoriesContractUpdateValidator()
        {
            #region Id

            RuleFor(x => x.CategoryId).NotNull()
                                      .WithState(x => Errors.IsNull("The category id can not be null."));

            RuleFor(x => x.CategoryId).NotEmpty()
                                      .WithState(x => Errors.IsNull("The category id can not be empty."));

            #endregion


            #region Name

            RuleFor(x => x.Name).NotNull()
                                .WithState(x => Errors.IsNull("The category name can not be null."));

            RuleFor(x => x.Name).NotEmpty()
                                .WithState(x => Errors.IsNull("The category name can not be empty."));

            RuleFor(x => x.Name).MaximumLength(Category.MaximumNameLength)
                                .WithState(x => Errors.IsNull($"The category name can not longer than {Category.MaximumNameLength} symbols."));

            #endregion
        }
    }
}
