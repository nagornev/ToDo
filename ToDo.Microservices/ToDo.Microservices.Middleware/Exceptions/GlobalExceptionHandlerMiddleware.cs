using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Middleware.Exceptions
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IGlobalExceptionHandlerConfiguration _configuration;

        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next,
                                                IGlobalExceptionHandlerConfiguration configuration,
                                                ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
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
                       Result.Failure(error => error.InternalServer($"The '{_configuration.ServiceName}' service is unavailable.")));

                _logger.LogError(exception, context.Request.Path);
            }
        }

        private async void Handle(HttpContext context, Result result)
        {
            context.Response.StatusCode = result.Error.Status;
            await context.Response.WriteAsync(result.ToString());
        }
    }
}
