using Nagornev.Querer.Http;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityResponseHandler : QuererHttpResponseMessageHandler<IdentityOutput>
    {
        protected override void SetContent(ContentHandler handler)
        {
            handler.SetContent(response => response.GetContent((JToken json) => json.ToObject<IdentityOutput>()!));
        }

        protected override IEnumerable<Scheme.Set> SetScheme(Scheme scheme)
        {
            return scheme.Configure(scheme.Content);
        }
    }
}
