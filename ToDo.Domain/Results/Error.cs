using System;
using System.Net;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [Serializable]
    public class Error : IError
    {
        public Error(HttpStatusCode status,
                     string key,
                     string message)
            : this((int)status, key, message)
        {
        }

        [JsonConstructor]
        public Error(int status,
                     string key,
                     string message)
        {
            Status = status;
            Key = key;
            Message = message;
        }

        [JsonPropertyName("code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public int Status { get; private set; }


        [JsonPropertyName("key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
        public string Key { get; private set; }

        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Message { get; private set; }
    }
}
