using System.Text.Json.Serialization;

namespace ToDo.Microservices.MQ.Queries.GetCategory
{
    [Serializable]
    public class GetCategoryProcedureRequest
    {
        public const string Queue = "get_category_rpc_requests";

        public GetCategoryProcedureRequest(Guid userId,
                                           Guid categoryId)
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
