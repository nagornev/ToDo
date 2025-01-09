using System;
using System.Net;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    [Serializable]
    public class ErrorField : Error, IError
    {
        public ErrorField(HttpStatusCode status,
                          string key,
                          string message,
                          string field) 
            : this((int)status, key, message, field)
        {
        }

        [JsonConstructor]
        public ErrorField(int status,
                          string key,
                          string message,
                          string field)
            : base(status, key, message)
        {
            Field = field;
        }

        [JsonPropertyName("field")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
        public string Field { get; }
    }
}
