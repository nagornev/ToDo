using System;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Identity.Domain.Models
{
    public class User
    {
        public const string PasswordPattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";

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
            return new User(id, email, password, access);
        }

        public static Result<User> Create(Guid id, string email, string password, Access access)
        {
            if (id == Guid.Empty)
                return Result<User>.Failure(Errors.IsInvalidArgument("The user id can not be empty.", nameof(id)));

            if (access is null)
                return Result<User>.Failure(Errors.IsInvalidArgument("The user access is null.", nameof(password)));

            return Result<User>.Successful(Constructor(id, email, password, access));
        }

        public static Result<User> New(string email, string password, Access access)
        {
            return Create(Guid.NewGuid(), email, password, access);
        }

        public static Result<User> NewUser(string email, string password)
        {
            return Create(Guid.NewGuid(), email, password, Access.Constructor(Role.User));
        }

        public static Result<User> NewSuper(string email, string password)
        {
            return Create(Guid.NewGuid(), email, password, Access.Constructor(Role.Super));
        }
    }
}
