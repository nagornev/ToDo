using System.Text.Json.Serialization;
using ToDo.Domain.Results.Errors;

namespace ToDo.Domain.Results
{
    [JsonDerivedType(typeof(DefaultError))]
    [JsonDerivedType(typeof(FieldError))]
    public interface IError
    {
        int Status { get; }

        string Message { get; }
    }
}