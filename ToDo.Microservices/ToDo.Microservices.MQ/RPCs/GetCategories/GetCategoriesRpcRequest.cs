using System.Text.Json.Serialization;

namespace ToDo.Microservices.MQ.RPCs.GetCategories
{
    [Serializable]
    public class GetCategoriesRpcRequest
    {
        public const string Queue = "get_categories_rpc_requests";

        public GetCategoriesRpcRequest(Guid userId)
        {
            UserId = userId;
        }

        [JsonPropertyName("userId")]
        public Guid UserId { get; private set; }
    }
}
