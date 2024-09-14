using System;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [Serializable]
    public class Error : IError
    {
        public Error(int code,
                     string key,
                     string message)
        {
            Code = code;
            Key = key;
            Message = message;
        }

        [JsonPropertyName("code")]
        public int Code { get; private set; }

        [JsonPropertyName("key")]
        public string Key { get; private set; }

        [JsonPropertyName("message")]
        public string Message { get; private set; }

    }
}
