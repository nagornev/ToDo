using ToDo.Domain.Results;
using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.Queries.GetCategories
{
    public class GetCategoriesProcedureResponse : Result<IEnumerable<CategoryMQ>>
    {
        public const string Queue = "get_categories_rpc_responses";

        public GetCategoriesProcedureResponse(bool success,
                                              IEnumerable<CategoryMQ> content = default,
                                              IError error = default)
            : base(success, content, error)
        {
        }
    }
}
