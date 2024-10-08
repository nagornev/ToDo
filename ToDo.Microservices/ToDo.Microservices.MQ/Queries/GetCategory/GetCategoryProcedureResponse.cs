using System.Text.Json.Serialization;
using ToDo.Domain.Results;
using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.Queries.GetCategory
{
    [Serializable]
    public class GetCategoryProcedureResponse
    {
        public const string Queue = "get_category_rpc_responses";

        public GetCategoryProcedureResponse(Result<CategoryMQ> result)
        {
            Result = result;
        }

        [JsonPropertyName("result")]
        public Result<CategoryMQ> Result { get; private set; }
    }
}
