using ToDo.Domain.Results;
using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.Queries.GetCategory
{
    [Serializable]
    public class GetCategoryProcedureResponse : Result<CategoryMQ>
    {
        public const string Queue = "get_category_rpc_responses";

        public GetCategoryProcedureResponse(bool success,
                                            CategoryMQ? content,
                                            IError error)
            : base(success, content, error)
        {
        }
    }
}
