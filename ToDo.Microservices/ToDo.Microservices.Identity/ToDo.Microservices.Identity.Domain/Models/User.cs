using System;
using System.Text.Json.Serialization;

namespace ToDo.Microservices.Identity.Domain.Models
{
    [Serializable]
    public class User
    {
        public const string PasswordPattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";

        [JsonConstructor]
        private User(Guid id,
                    string email,
                    string password,
                    Access access)
        {
            Id = id;
            Email = email;
            Password = password;
            Access = access;
        }

        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public Access Access { get; private set; }

        public static User Constructor(Guid id, string email, string password, Access access)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"The user id({id}) can not bee null or empty.");

            if (access == null)
                throw new ArgumentNullException("The user access can not be null.");

            return new User(id, email, password, access);
        }

        public static User New(string email, string password, Access access)
        {
            return Constructor(Guid.NewGuid(), email, password, access);
        }

        public static User NewUser(string email, string password)
        {
            return Constructor(Guid.NewGuid(), email, password, Access.Constructor(Role.User));
        }

        public static User NewSuper(string email, string password)
        {
            return Constructor(Guid.NewGuid(), email, password, Access.Constructor(Role.Super));
        }
    }
}
