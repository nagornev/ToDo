using System;

namespace ToDo.Microservices.Categories.Domain.Models
{
    public class User
    {
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
