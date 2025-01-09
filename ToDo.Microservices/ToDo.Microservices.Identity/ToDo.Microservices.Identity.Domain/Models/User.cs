﻿using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Identity.Domain.Models
{
    [Serializable]
    public class User
    {
        public const string EmailExpression = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        public const string PasswordExpression = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";

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

        public static Result<User> Constructor(Guid id, string email, string password, Access access, Func<string> passwordHashFactory = null)
        {
            if (id == Guid.Empty)
                return Result<User>.Failure(error => error.NullOrEmpty("The user`s ID can`t be null or empty.", nameof(Id)));

            if (!Regex.IsMatch(email, EmailExpression))
                return Result<User>.Failure(error => error.InvalidArgument("The user`s email does not match the regular email expression.", nameof(Email)));

            if (!(passwordHashFactory is null) && !Regex.IsMatch(password, PasswordExpression))
                return Result<User>.Failure(error => error.InvalidArgument("The user`s password does not match the regular password expression.", nameof(Password)));

            if (access == null)
                return Result<User>.Failure(error => error.NullOrEmpty("The user`s access can`t be null.", nameof(Access)));

            return Result<User>.Successful(new User(id, email, passwordHashFactory?.Invoke() ?? password, access));
        }

        public static Result<User> New(string email, string password, Func<string> passwordHashFactory, Access access)
        {
            return Constructor(Guid.NewGuid(), email, password, access, passwordHashFactory);
        }

        public static Result<User> NewUser(string email, string password, Func<string> passwordHashFactory)
        {
            return Constructor(Guid.NewGuid(), email, password, Access.Constructor(Role.User), passwordHashFactory);
        }

        public static Result<User> NewSuper(string email, string password, Func<string> passwordHashFactory)
        {
            return Constructor(Guid.NewGuid(), email, password, Access.Constructor(Role.Super), passwordHashFactory);
        }
    }
}
