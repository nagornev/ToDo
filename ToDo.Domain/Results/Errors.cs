using System;

namespace ToDo.Domain.Results
{
    public static class Errors
    {
        public delegate TError OnErrorCreationDelegate<TError>(string key, string message)
            where TError : IError;

        private const string _isMessageKey = "is.message";

        private const string _isInvalidArgumentKey = "is.invalid.argument";

        private const string _isNullKey = "is.null";

        private const string _isUnautorizatedKey = "is.unauthorizated";

        private const string _isForbiddenKey = "is.forbidden";

        public static TError Create<TError>(Action<TError> callback)
            where TError : IError
        {
            TError error = Activator.CreateInstance<TError>();

            callback.Invoke(error);

            return error;
        }

        #region IsUnauthorizated

        public static IError IsUnauthorizated(string message)
        {
            return new Error(_isUnautorizatedKey,
                             message);
        }

        #endregion

        #region IsFrobidden

        public static IError IsForbidden(string message)
        {
            return new Error(_isForbiddenKey,
                             message);
        }

        #endregion

        #region IsMessage

        public static IError IsMessage(string message)
        {
            return new Error(_isMessageKey,
                             message);
        }

        #endregion

        #region IsInvalidArgument

        public static IError IsInvalidArgument(string message)
        {
            return new Error(_isInvalidArgumentKey,
                             message);
        }

        public static IError IsInvalidArgument(string message, string field)
        {
            return new ErrorField(_isInvalidArgumentKey,
                                  message,
                                  field);
        }

        #endregion

        #region IsNull

        public static IError IsNull(string message)
        {
            return new Error(_isNullKey,
                             message);
        }

        public static IError IsNull(string message, string field)
        {
            return new ErrorField(_isNullKey,
                                  message,
                                  field);
        }

        #endregion
    }
}
