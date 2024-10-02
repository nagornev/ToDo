using System.Text.Json.Serialization;
using ToDo.Domain.Results;
using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.RPCs.GetCategory
{
    [Serializable]
    public class GetCategoryRpcResponse
    {
        public const string Queue = "get_category_rpc_responses";

        public GetCategoryRpcResponse(Result<CategoryMQ> result)
        {
            Result = result;
        }

        [JsonPropertyName("result")]
        public Result<CategoryMQ> Result { get; private set; }
    }
}
