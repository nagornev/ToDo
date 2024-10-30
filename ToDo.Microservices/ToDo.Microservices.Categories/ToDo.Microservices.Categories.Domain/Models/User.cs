using System;
using System.Text.Json.Serialization;

namespace ToDo.Microservices.Categories.Domain.Models
{
    [Serializable]
    public class User
    {
        [JsonConstructor]
        private User(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

        public static User Constructor(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("The user id can not be empty.");

            return new User(id);
        }
    }
}
