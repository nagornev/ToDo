using System;

namespace ToDo.Microservices.Entries.Domain.Models
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
            if(id == Guid.Empty)
                throw new ArgumentNullException("The user id can not be empty.");

            return new User(id);
        }
    }
}
