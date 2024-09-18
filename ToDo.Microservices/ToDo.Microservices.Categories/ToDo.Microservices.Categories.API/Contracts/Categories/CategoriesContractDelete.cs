namespace ToDo.Microservices.Categories.API.Contracts.Categories
{
    public class CategoriesContractDelete
    {
        public CategoriesContractDelete(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public Guid CategoryId { get; private set; }
    }
}
