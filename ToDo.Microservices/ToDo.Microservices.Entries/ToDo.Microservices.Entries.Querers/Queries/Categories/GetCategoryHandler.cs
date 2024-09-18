using Nagornev.Querer.Http;
using Newtonsoft.Json.Linq;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Querers.Queries.Categories
{
    public class GetCategoryHandler : QueryHandler<Result<Category>>
    {
        protected override void SetContent(ContentHandler handler)
        {
            handler.SetContent(response => response.GetContent((JToken json) => json.ToObject<Result<Category>>(Serializer)!));
        }
    }
}
