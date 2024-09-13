using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [JsonDerivedType(typeof(Error))]
    [JsonDerivedType(typeof(ErrorField))]
    public interface IError
    {
        int Code { get; }

        string Key { get; }

        string Message { get; }
    }
}