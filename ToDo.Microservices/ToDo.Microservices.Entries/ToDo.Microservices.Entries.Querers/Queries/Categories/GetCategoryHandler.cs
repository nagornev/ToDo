using Nagornev.Querer.Http;
using Newtonsoft.Json.Linq;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Querers.Queries.Categories
{
    public class GetCategoryHandler : QueryHandler<Category>
    {
        protected override void SetContent(ContentHandler handler)
        {
            handler.SetContent(response => response.GetContent((JToken json) => json.SelectToken(ContentToken)!
                                                                                    .ToObject<Category>()!));
        }
    }
}
