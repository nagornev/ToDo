using System.Text.Json.Serialization;

namespace ToDo.Microservices.MQ.Queries.GetCategories
{
    [Serializable]
    public class GetCategoriesProcedureRequest
    {
        public const string Queue = "get_categories_rpc_requests";

        public GetCategoriesProcedureRequest(Guid userId)
        {
            UserId = userId;
        }

        [JsonPropertyName("userId")]
        public Guid UserId { get; private set; }
    }
}
