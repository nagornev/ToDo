using System.Net;
using System.Text.Json.Serialization;

namespace ToDo.Domain.Results.Errors
{
    public class FieldError : DefaultError
    {
        public FieldError(HttpStatusCode status,
                          string message,
                          string field)
            : base(status, message)
        {
            Field = field;
        }

        [JsonPropertyName("field")]
        public string Field { get; private set; }
    }
}
