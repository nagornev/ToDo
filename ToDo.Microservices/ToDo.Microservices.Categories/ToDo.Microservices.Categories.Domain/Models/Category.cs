using System;

namespace ToDo.Microservices.Categories.Domain.Models
{
    public class Category
    {
        public const int MaximumNameLength = 20;

        private Category(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public static Category Constructor(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("The category id can not be empty.");

            return new Category(id, name);
        }

        public static Category New(string name)
        {
            return Constructor(Guid.NewGuid(), name);
        }
    }
}
