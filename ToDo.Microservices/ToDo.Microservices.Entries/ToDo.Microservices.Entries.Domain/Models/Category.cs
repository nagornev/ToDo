using System;
using System.Text.Json.Serialization;

namespace ToDo.Microservices.Entries.Domain.Models
{
    [Serializable]
    public class Category
    {
        [JsonConstructor]
        public Category(Guid id,
                        string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }
    }
}
