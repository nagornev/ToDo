using Microsoft.AspNetCore.Http;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Middleware.Exceptions
{
    public class GlobalExceptionHandlerMiddleware
    {
        private RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
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
                       Result.Failure(Errors.IsInternalServer(exception.StackTrace)));
            }
        }

        private async void Handle(HttpContext context, Result result)
        {
            context.Response.StatusCode = result.Error.Code;
            await context.Response.WriteAsync(result.ToString());
        }
    }
}
