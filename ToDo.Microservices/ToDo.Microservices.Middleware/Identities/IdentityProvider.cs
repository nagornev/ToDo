using Microsoft.AspNetCore.Http;
using Nagornev.Querer.Http;
using System.Security.Claims;
using ToDo.Domain.Results;
using ToDo.Extensions;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityProvider : IIdentityProvider
    {
        private const string _authenticationType = "Identity";

        private IQuererHttpClientFactory _factory;
        private IIdentityAttributeProvider _attributeProvider;

        public IdentityProvider(IQuererHttpClientFactory factory,
                                IIdentityAttributeProvider attributeProvider)
        {
            _factory = factory;
            _attributeProvider = attributeProvider;
        }

        public async Task Identify(RequestDelegate next,
                                   HttpContext context)
        {
            if (!_attributeProvider.TryGet(context, out IdentityAttribute attribute))
            {
                await next.Invoke(context);
                return;
            }

            IdentityRequestCompiler compiler = new IdentityRequestCompiler(attribute);
            IdentityResponseHandler handler = new IdentityResponseHandler();

            using (QuererHttpClient quererClient = _factory.Create())
            {
                await quererClient.SendAsync(compiler, handler);
            }

            Result<Guid?> identityResult = handler.Content;

            if (!identityResult.Success)
            {
                BadRequest(context.Response, handler.Content);
                return;
            }

            context.User = GetPricipial(handler.Content);
            await next.Invoke(context);
        }

        private async void BadRequest(HttpResponse response, Result output)
        {
            response.StatusCode = output.Error!.Code;
            await response.WriteAsync(output.ToString());
        }

        private ClaimsPrincipal GetPricipial(Result<Guid?> output)
        {
            Claim[] claims =
            {
                  new Claim(IdentityDefaults.Subject, output.Content.ToString()!),
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, _authenticationType);

            return new ClaimsPrincipal(identity);
        }
    }
}
