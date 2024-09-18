namespace ToDo.Microservices.Categories.API.Contracts.Categories
{
    public class CategoriesContractCreate
    {
        public CategoriesContractCreate(string? name)
        {
            Name = name;
        }

        public string? Name { get; private set; }
    }
}
