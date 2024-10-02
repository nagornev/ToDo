using System.Text.Json.Serialization;

namespace ToDo.Microservices.MQ.Models
{

    [Serializable]
    public class UserMQ
    {
        public UserMQ(Guid id,
                      string email)
        {
            Id = id;
            Email = email;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("email")]
        public string Email { get; private set; }
    }
}
