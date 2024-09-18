namespace ToDo.Microservices.Categories.API.Contracts.Categories
{
    public class CategoriesContractUpdate
    {
        public CategoriesContractUpdate(Guid categoryId,
                                        string? name)
        {
            CategoryId = categoryId;
            Name = name;
        }

        public Guid CategoryId { get; private set; }

        public string? Name { get; private set; }
    }
}
