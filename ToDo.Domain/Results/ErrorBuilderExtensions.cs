using System.Net;

namespace ToDo.Domain.Results
{
    public static class ErrorBuilderExtensions
    {
        public static ErrorBuilder InternalServer(this ErrorBuilder builder,
                                                  string message)
        {
            builder.SetStatus(HttpStatusCode.InternalServerError)
                   .SetKey(ErrorKeys.InternalServer)
                   .SetMessage(message);

            return builder;
        }

        public static ErrorBuilder NullOrEmpty(this ErrorBuilder builder,
                                               string message,
                                               string field = default)
        {
            builder.SetStatus(HttpStatusCode.BadRequest)
                   .SetKey(ErrorKeys.NullOrEmpty)
                   .SetMessage(message)
                   .UseFactory((s, k, m) => new ErrorField(s, k, m, field));

            return builder;
        }

        public static ErrorBuilder InvalidArgument(this ErrorBuilder builder,
                                                   string message,
                                                   string field = default)
        {
            builder.SetStatus(HttpStatusCode.BadRequest)
                   .SetKey(ErrorKeys.InvalidArgument)
                   .SetMessage(message)
                   .UseFactory((s, k, m) => new ErrorField(s, k, m, field));

            return builder;
        }

        public static ErrorBuilder InvalidSignIn(this ErrorBuilder builder,
                                          string message = "Invalid sign in.")
        {
            builder.SetStatus(HttpStatusCode.BadRequest)
                   .SetKey(ErrorKeys.InvalidSignIn)
                   .SetMessage(message);

            return builder;
        }
        public static ErrorBuilder Unauthorizated(this ErrorBuilder builder,
                                                  string message = "Unauthorizated.")
        {
            builder.SetStatus(HttpStatusCode.Unauthorized)
                   .SetKey(ErrorKeys.Unauthorizated)
                   .SetMessage(message);

            return builder;
        }

        public static ErrorBuilder Forbidden(this ErrorBuilder builder,
                                             string message = "Forbidden.")
        {
            builder.SetStatus(HttpStatusCode.Forbidden)
                   .SetKey(ErrorKeys.Forbidden)
                   .SetMessage(message);

            return builder;
        }
    }
}
