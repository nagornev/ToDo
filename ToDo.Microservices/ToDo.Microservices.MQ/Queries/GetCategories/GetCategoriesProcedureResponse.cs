using System.Text.Json.Serialization;
using ToDo.Domain.Results;
using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.Queries.GetCategories
{
    public class GetCategoriesProcedureResponse
    {
        public const string Queue = "get_categories_rpc_responses";

        public GetCategoriesProcedureResponse(Result<IEnumerable<CategoryMQ>> result)
        {
            Result = result;
        }

        [JsonPropertyName("result")]
        public Result<IEnumerable<CategoryMQ>> Result { get; private set; }
    }
}
