using Microsoft.AspNetCore.Http;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Middleware.Exceptions
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IGlobalExceptionHandlerConfiguration _configuration;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next,
                                                IGlobalExceptionHandlerConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                Handle(context,
                       Result.Failure(Errors.IsInternalServer($"The '{_configuration.ServiceName}' service is unavailable.")));
            }
        }

        private async void Handle(HttpContext context, Result result)
        {
            context.Response.StatusCode = result.Error.Code;
            await context.Response.WriteAsync(result.ToString());
        }
    }
}
