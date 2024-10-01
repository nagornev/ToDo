using System.Text.Json.Serialization;
using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.RPCs.GetCategory
{
    [Serializable]
    public class GetCategoryRpcResponse
    {
        public const string Queue = "get_category_rpc_responses";

        public GetCategoryRpcResponse(CategoryMQ? category)
        {
            Category = category;
        }

        [JsonPropertyName("category")]
        public CategoryMQ? Category { get; private set; }
    }
}
