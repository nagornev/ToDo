﻿using System;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Extensions;

namespace ToDo.Microservices.Entries.Domain.Models
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

        public static Result<User> Constructor(Guid id)
        {
            if (id == Guid.Empty)
                return Result<User>.Failure(error => error.NullOrEmpty("The user ID can`t be null or empty.", nameof(Id)));

            return Result<User>.Successful(new User(id));
        }
    }
}
