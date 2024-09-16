using System;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [Serializable]
    public class Result<T> : Result
    {
        public Result(bool success, T content, IError error)
            : base(success, error)
        {
            Content = content;
        }

        [JsonPropertyName("content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Content { get; private set; }

        private static Result<T> Create(bool success, T content, IError error)
        {
            return new Result<T>(success, content, error);
        }

        public new static Result<T> Failure(IError error)
        {
            return Create(false, default, error);
        }

        public static Result<T> Successful(T content)
        {
            return Create(true, content, default);
        }
    }
}
