using System.Text.Json.Serialization;

namespace ToDo.Microservices.MQ.Publishers
{
    [Serializable]
    public abstract class UserPublish
    {
        public UserPublish(Guid userId)
        {
            UserId = userId;
        }

        [JsonPropertyName("userId")]
        public Guid UserId { get; private set; }
    }
}
