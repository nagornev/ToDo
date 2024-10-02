using System.Text.Json.Serialization;
using ToDo.Domain.Results;
using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.RPCs.GetCategories
{
    public class GetCategoriesRpcResponse
    {
        public const string Queue = "get_categories_rpc_responses";

        public GetCategoriesRpcResponse(Result<IEnumerable<CategoryMQ>> result)
        {
            Result = result;
        }

        [JsonPropertyName("result")]
        public Result<IEnumerable<CategoryMQ>> Result { get; private set; }

        //[JsonPropertyName("categories")]
        //public IEnumerable<CategoryMQ> Categories { get; private set; }
    }
}
