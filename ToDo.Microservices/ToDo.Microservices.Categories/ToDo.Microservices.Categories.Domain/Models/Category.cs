using System;
using System.Text.Json.Serialization;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Categories.Domain.Models
{
    [Serializable]
    public class Category
    {
        public const int NameMaximumLength = 50;

        [JsonConstructor]
        private Category(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public static Result<Category> Constructor(Guid id, string name)
        {
            if (id == Guid.Empty)
                return Result<Category>.Failure(error => error.NullOrEmpty("The category ID can`t be null or empty.", nameof(Id)));

            if (name.Length > NameMaximumLength)
                return Result<Category>.Failure(error => error.InvalidArgument($"The category name can`t be more than {NameMaximumLength} symbols.", nameof(Name)));

            return Result<Category>.Successful(new Category(id, name));
        }

        public static Result<Category> New(string name)
        {
            return Constructor(Guid.NewGuid(), name);
        }
    }
}
