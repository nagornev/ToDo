using System.Text.Json.Serialization;
using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.RPCs.GetCategories
{
    public class GetCategoriesRpcResponse
    {
        public const string Queue = "get_categories_rpc_responses";

        public GetCategoriesRpcResponse(List<CategoryMQ> categories)
        {
            Categories = categories;
        }

        [JsonPropertyName("categories")]
        public List<CategoryMQ> Categories { get; private set; }
    }
}
