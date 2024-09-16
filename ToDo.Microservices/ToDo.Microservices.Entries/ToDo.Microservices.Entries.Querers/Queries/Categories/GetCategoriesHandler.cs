using Nagornev.Querer.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDo.Domain.Results;
using ToDo.Extensions.Converters;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Querers.Queries.Categories
{
    public class GetCategoriesHandler : QueryHandler<Result<IEnumerable<Category>>>
    {
        private JsonSerializer _serializer;

        public GetCategoriesHandler()
        {
            _serializer = new JsonSerializer();
            _serializer.Converters.Add(new Interface2ClassConverter<IEnumerable<Category>, List<Category>>());
        }

        protected override void SetContent(ContentHandler handler)
        {
            handler.SetContent(response => response.GetContent((JToken json) => json.ToObject<Result<IEnumerable<Category>>>(_serializer)!));
        }
    }
}
