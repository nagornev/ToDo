using System;
using System.Text.Json.Serialization;

namespace ToDo.Microservices.Entries.Domain.Models
{
    public class Category
    {
        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        public Category(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
