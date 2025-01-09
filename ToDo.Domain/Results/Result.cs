using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [Serializable]
    public class Result
    {
        [JsonConstructor]
        protected Result(bool success,
                         IError error = default)
        {
            Success = success;
            Error = error;
        }

        [JsonPropertyName("success")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public bool Success { get; private set; }

        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
        public IError Error { get; private set; }

        private static Result Constructor(bool success, IError error = default)
        {
            return new Result(success, error);
        }

        public static Result Successful()
        {
            return Constructor(success: true);
        }

        public static Result Failure()
        {
            return Constructor(success: false);
        }

        public static Result Failure(IError error)
        {
            return Constructor(success: false,
                               error: error);
        }

        public static Result Failure(Action<ErrorBuilder> options)
        {
            ErrorBuilder builder = new ErrorBuilder();

            options.Invoke(builder);

            return Failure(error: builder.Build());
        }

        #region Overrides

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public static implicit operator bool(Result result)
        {
            return result.Success;
        }

        #endregion
    }
}
