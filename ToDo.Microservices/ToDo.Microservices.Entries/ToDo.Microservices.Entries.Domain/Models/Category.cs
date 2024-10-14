using System;
using System.Text.Json.Serialization;

namespace ToDo.Microservices.Entries.Domain.Models
{
    public class Category
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        [JsonConstructor]
        public Category(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
