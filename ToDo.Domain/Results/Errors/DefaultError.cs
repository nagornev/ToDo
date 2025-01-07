using System;
using System.Net;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results.Errors
{
    [Serializable]
    public class DefaultError : IError
    {
        public DefaultError(HttpStatusCode status,
                     string message)
        {
            Status = (int)status;
            Message = message;
        }

        [JsonPropertyName("code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Status { get; private set; }

        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Message { get; private set; }
    }
}
