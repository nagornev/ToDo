using System.Text.Json.Serialization;

namespace ToDo.Microservices.MQ.Models
{

    [Serializable]
    public class UserMQ
    {
        public UserMQ(Guid id)
        {
            Id = id;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; private set; }
    }
}
