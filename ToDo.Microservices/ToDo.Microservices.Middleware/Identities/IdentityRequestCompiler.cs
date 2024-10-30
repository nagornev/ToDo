using Nagornev.Querer.Http;
using System.Text;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityRequestCompiler : QuererHttpRequestMessageCompiler
    {
        private IdentityContent _content;

        public IdentityRequestCompiler(IdentityAttribute attribute)
        {
            _content = new IdentityContent(attribute.Permissions);
        }

        protected override void SetMethod(MethodCompiler compiler)
        {
            compiler.Set(HttpMethod.Post);
        }

        protected override void SetUrl(UrlCompiler compiler)
        {
            compiler.Set("http://identity_microservice:8080/Identities/Validate");
        }

        protected override void SetContent(ContentCompiler compiler)
        {
            compiler.Set(_content.ToString(), Encoding.UTF8);
        }

        protected override IEnumerable<Scheme.Set> SetScheme(Scheme scheme)
        {
            return scheme.Configure(scheme.Method, scheme.Url, scheme.Content);
        }
    }
}
