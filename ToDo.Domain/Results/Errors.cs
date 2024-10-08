using System;

namespace ToDo.Domain.Results
{
    public static class Errors
    {
        public delegate TError OnErrorCreationDelegate<TError>(string key, string message)
            where TError : IError;

        public const string IsMessageKey = "is.message";

        public const string IsInvalidArgumentKey = "is.invalid.argument";

        public const string IsNullKey = "is.null";

        public const string IsUnautorizatedKey = "is.unauthorizated";

        public const string IsForbiddenKey = "is.forbidden";

        public const string IsInternalServerKey = "is.internal.server";

        public static TError Create<TError>(Action<TError> callback)
            where TError : IError
        {
            TError error = Activator.CreateInstance<TError>();

            callback.Invoke(error);

            return error;
        }

        #region IsInternalServer

        public static IError IsInternalServer(string message)
        {
            return new Error(500,
                             IsInternalServerKey,
                             message);
        }

        #endregion

        #region IsUnauthorizated

        public static IError IsUnauthorizated(string message)
        {
            return new Error(401,
                             IsUnautorizatedKey,
                             message);
        }

        #endregion

        #region IsFrobidden

        public static IError IsForbidden(string message)
        {
            return new Error(403,
                             IsForbiddenKey,
                             message);
        }

        #endregion

        #region IsMessage

        public static IError IsMessage(string message)
        {
            return new Error(400,
                             IsMessageKey,
                             message);
        }

        #endregion

        #region IsInvalidArgument

        public static IError IsInvalidArgument(string message)
        {
            return new Error(400,
                             IsInvalidArgumentKey,
                             message);
        }

        public static IError IsInvalidArgument(string message, string field)
        {
            return new ErrorField(400,
                                  IsInvalidArgumentKey,
                                  message,
                                  field);
        }

        #endregion

        #region IsNull

        public static IError IsNull(string message)
        {
            return new Error(400,
                             IsNullKey,
                             message);
        }

        public static IError IsNull(string message, string field)
        {
            return new ErrorField(400,
                                  IsNullKey,
                                  message,
                                  field);
        }

        #endregion
    }
}
