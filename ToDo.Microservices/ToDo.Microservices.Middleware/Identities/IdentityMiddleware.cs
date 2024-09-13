using Microsoft.AspNetCore.Http;
using Nagornev.Querer.Http;
using System.Security.Claims;
using System.Text;

namespace ToDo.Microservices.Middleware.Identities
{
    public abstract class IdentityMiddleware : IMiddleware
    {
        private const string _authenticationType = "Identity";

        private QuererHttpClient _quererClient;

        public IdentityMiddleware()
        {
            _quererClient = new QuererHttpClient();
        }

        protected abstract bool TryGetIdentity(HttpContext context, out IdentityAttribute attribute);

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!TryGetIdentity(context, out IdentityAttribute attribute))
            {
                await next.Invoke(context);
                return;
            }

            IdentityRequestCompiler compiler = new IdentityRequestCompiler(context.Request.Cookies, attribute);
            IdentityResponseHandler handler = new IdentityResponseHandler();

            await _quererClient.SendAsync(compiler, handler);

            if (handler.Content.Success)
            {
                context.User = GetPricipial(handler.Content);
                await next.Invoke(context);
            }
            else
            {
                BadRequest(context.Response, handler.Content);
                return;
            }
        }

        private ClaimsPrincipal GetPricipial(IdentityOutput output)
        {
            Claim[] claims =
            {
                  new Claim(IdentityDefaults.Subject, output.User.ToString()),
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, _authenticationType);

            return new ClaimsPrincipal(identity);
        }

        private async void BadRequest(HttpResponse response, IdentityOutput output)
        {
            byte[] body = Encoding.UTF8.GetBytes(output.ToString()); 

            response.StatusCode = output.Error!.Code;
            await response.WriteAsync(output.ToString());
        }
    }
}
