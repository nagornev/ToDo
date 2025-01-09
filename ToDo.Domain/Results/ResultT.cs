using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [Serializable]
    public class Result<T> : Result
    {
        [JsonConstructor]
        protected Result(bool success,
                         T content = default,
                         IError error = default)
            : base(success, error)
        {
            Content = content;
        }

        [JsonPropertyName("content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
        public T Content { get; private set; }

        private static Result<T> Constructor(bool success,
                                             T content = default,
                                             IError error = default)
        {
            return new Result<T>(success, content, error);
        }

        public static Result<T> Successful(T content)
        {
            return Constructor(success: true,
                               content: content);
        }

        public new static Result<T> Failure()
        {
            return Constructor(success: false);
        }

        public new static Result<T> Failure(IError error)
        {
            return Constructor(success: false,
                               error: error);
        }

        public new static Result<T> Failure(Action<ErrorBuilder> options)
        {
            ErrorBuilder builder = new ErrorBuilder();

            options.Invoke(builder);

            return Failure(builder.Build());
        }

        public static Result<T> Deserialize(string content)
        {
            return JsonSerializer.Deserialize<Result<T>>(content);
        }

        #region Overrides

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        #endregion
    }
}
