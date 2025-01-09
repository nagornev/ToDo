using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [JsonDerivedType(typeof(Error), 0)]
    [JsonDerivedType(typeof(ErrorField), 1)]
    public interface IError
    {
        int Status { get; }

        string Key { get; }

        string Message { get; }
    }
}