using System.Net;
using ToDo.Domain.Results.Errors;

namespace ToDo.Domain.Results.Extensions
{
    public static class ErrorBuilderExtensions
    {
        public static ErrorBuilder InternalServer(this ErrorBuilder builder, string message = "")
        {
            builder.SetStatus(HttpStatusCode.InternalServerError)
                   .SetMessage(message);

            return builder;
        }

        public static ErrorBuilder NullOrEmpty(this ErrorBuilder builder, string message = "The field can`t be null or empty.", string field = "")
        {
            builder.SetStatus(HttpStatusCode.BadRequest)
                   .SetMessage(message)
                   .UseFactory((s, m) => new FieldError(s, m, field));

            return builder;
        }

        public static ErrorBuilder InvalidArgument(this ErrorBuilder builder, string message = "The argument has an invalid value.", string field = "")
        {
            builder.SetStatus(HttpStatusCode.BadRequest)
                   .SetMessage(message)
                   .UseFactory((s, m) => new FieldError(s, m, field));

            return builder;
        }

        public static ErrorBuilder SignIn(this ErrorBuilder builder, string message = "Invalid signin. Please check your email and password and try again.")
        {
            builder.SetStatus(HttpStatusCode.BadRequest)
                   .SetMessage(message);

            return builder;
        }
        public static ErrorBuilder Unauthorizated(this ErrorBuilder builder, string message = "Unauthorizated.")
        {
            builder.SetStatus(HttpStatusCode.Unauthorized)
                   .SetMessage(message);

            return builder;
        }

        public static ErrorBuilder Forbidden(this ErrorBuilder builder, string message = "Forbidden.")
        {
            builder.SetStatus(HttpStatusCode.Forbidden)
                   .SetMessage(message);

            return builder;
        }
    }
}
