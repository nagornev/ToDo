using System.Net;
using ToDo.Domain.Results.Errors;

namespace ToDo.Domain.Results
{
    public class ErrorBuilder
    {
        public delegate IError ErrorFactoryDelegate(HttpStatusCode status, string message);

        private HttpStatusCode _status;

        private string _message;

        private ErrorFactoryDelegate _factory;

        public ErrorBuilder SetStatus(HttpStatusCode status)
        {
            _status = status;

            return this;
        }

        public ErrorBuilder SetMessage(string message)
        {
            _message = message;

            return this;
        }

        public ErrorBuilder UseFactory(ErrorFactoryDelegate factory)
        {
            _factory = factory;

            return this;
        }

        internal IError Build()
        {
            return _factory?.Invoke(_status, _message) ?? new DefaultError(_status, _message);
        }
    }
}
