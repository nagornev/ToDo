using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [Serializable]
    public class Result
    {
        protected Result(bool success, IError error)
        {
            Success = success;
            Error = error;
        }

        public bool Success { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IError Error { get; private set; }

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
    }
}
