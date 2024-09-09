using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [JsonDerivedType(typeof(Error))]
    [JsonDerivedType(typeof(ErrorField))]
    public interface IError
    {
        string Key { get; }

        string Message { get; }
    }
}