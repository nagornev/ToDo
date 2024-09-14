using Nagornev.Querer.Http;
using Newtonsoft.Json.Linq;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Querers.Queries.Categories
{
    public class GetCategoriesHandler : QueryHandler<IEnumerable<Category>>
    {
        protected override void SetContent(ContentHandler handler)
        {
            handler.SetContent(response => response.GetContent((JToken json) => json.SelectToken(ContentToken)!
                                                                                    .ToObject<List<Category>>()!));
        }
    }
}
