using System.Text.Json.Serialization;

namespace ToDo.Microservices.MQ.Publishers
{
    public class DeleteCategoryPublishMessage
    {
        public const string Exchange = "delete_category_exchange";

        public DeleteCategoryPublishMessage(Guid userId, Guid categoryId)
        {
            UserId = userId;
            CategoryId = categoryId;
        }

        [JsonPropertyName("userId")]
        public Guid UserId { get; private set; }

        [JsonPropertyName("categoryId")]
        public Guid CategoryId { get; private set; }
    }
}
