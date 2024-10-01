using System.Text.Json.Serialization;

namespace ToDo.Domain.Results
{
    public class ErrorField : Error
    {
        public ErrorField(int code,
                          string key,
                          string message,
                          string field)
            : base(code, key, message)
        {
            Field = field;
        }

        [JsonPropertyName("field")]
        public string Field { get; private set; }
    }
}
