using Nagornev.Querer.Http;
using System.Net;
using System.Text;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityRequestCompiler : QuererHttpRequestMessageCompiler
    {
        private IdentityContent _content;

        public IdentityRequestCompiler(IEnumerable<KeyValuePair<string,string>> cookies, IdentityAttribute attribute)
        {
            _content = new IdentityContent(cookies, attribute.Permissions);
        }

        protected override void SetMethod(MethodCompiler compiler)
        {
            compiler.Set(HttpMethod.Post);
        }

        protected override void SetUrl(UrlCompiler compiler)
        {
            compiler.Set("http://localhost:5254/Identities/Validate");
        }

        protected override void SetContent(ContentCompiler compiler)
        {
            compiler.Set(_content.GetContent(), Encoding.UTF8);
        }
    }
}
