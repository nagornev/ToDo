using FluentValidation;
using ToDo.Domain.Results;

namespace ToDo.Extensions.Validator
{
    public static class FluentValidatorExtensions
    {
        public static bool Validate<T>(this IValidator<T> validator, T obj, out Result result)
        {
            var validation = validator.Validate(obj);

            result = validation.IsValid ?
                        Result.Successful() :
                        Result.Failure((IError)validation.Errors.First().CustomState);

            return result.Success;
        }
    }
}
