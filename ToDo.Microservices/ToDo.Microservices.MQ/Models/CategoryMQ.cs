using System.Text.Json.Serialization;

namespace ToDo.Microservices.MQ.Models
{
    [Serializable]
    public class CategoryMQ
    {
        public CategoryMQ(Guid id,
                          string name)
        {
            Id = id;
            Name = name;
        }

        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }
    }
}
