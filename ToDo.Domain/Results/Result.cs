using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [Serializable]
    public class Result
    {
        public Result(bool success,
                      IError error = null)
        {
            if (!success && error == null)
                throw new ArgumentException($"The success result can not be true, if error has the value.");

            Success = success;
            Error = error;
        }

        [JsonPropertyName("success")]
        public bool Success { get; private set; }

        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IError Error { get; private set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        private static Result Create(bool success, IError error = default)
        {
            return new Result(success, error);
        }

        public static Result Failure(IError error = default)
        {
            return Create(false, error);
        }

        public static Result Successful()
        {
            return Create(true);
        }

        public static implicit operator bool(Result result)
        {
            return result.Success;
        }
    }
}
