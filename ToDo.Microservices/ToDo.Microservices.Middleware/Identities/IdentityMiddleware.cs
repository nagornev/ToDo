﻿using Microsoft.AspNetCore.Http;
using Nagornev.Querer.Http;
using System.Security.Claims;
using ToDo.Domain.Results;
using ToDo.Extensions;

namespace ToDo.Microservices.Middleware.Identities
{
    public abstract class IdentityMiddleware
    {
        private const string _authenticationType = "Identity";

        private readonly RequestDelegate _next;

        public IdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        protected abstract bool TryGetIdentity(HttpContext context, out IdentityAttribute attribute);

        protected virtual async Task<Result> Check(Guid userId)
        {
            return await Task.FromResult(Result.Successful());
        }

        public async Task InvokeAsync(HttpContext context, IQuererHttpClientFactory factory)
        {
            if (!TryGetIdentity(context, out IdentityAttribute attribute))
            {
                await _next.Invoke(context);
                return;
            }

            QuererHttpClient quererClient = factory.Create();
            IdentityRequestCompiler compiler = new IdentityRequestCompiler(attribute);
            IdentityResponseHandler handler = new IdentityResponseHandler();

            await quererClient.SendAsync(compiler, handler);

            Result<Guid?> identityResult = handler.Content;

            if (!identityResult.Success)
            {
                BadRequest(context.Response, handler.Content);
                return;
            }

            Result checkResult = await Check((Guid)identityResult.Content);

            if (!checkResult.Success)
            {
                BadRequest(context.Response, checkResult);
                return;
            }

            context.User = GetPricipial(handler.Content);
            await _next.Invoke(context);
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

        private async void BadRequest(HttpResponse response, Result output)
        {
            response.StatusCode = output.Error!.Code;
            await response.WriteAsync(output.ToString());
        }
    }
}
