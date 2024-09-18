using FluentValidation;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Categories.API.Contracts.Categories
{
    public class CategoriesContractDeleteValidator:AbstractValidator<CategoriesContractDelete>
    {
        public CategoriesContractDeleteValidator()
        {
            #region Id

            RuleFor(x => x.CategoryId).NotNull()
                                      .WithState(x => Errors.IsNull("The category id can not be null."));

            RuleFor(x => x.CategoryId).NotEmpty()
                                      .WithState(x => Errors.IsNull("The category id can not be empty."));

            #endregion
        }
    }
}
