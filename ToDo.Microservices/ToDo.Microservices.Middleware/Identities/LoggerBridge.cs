using Microsoft.Extensions.Logging;
using Nagornev.Querer;

namespace ToDo.Microservices.Middleware.Identities
{
    public class LoggerBridge<T> : Logger<T>, IQuererLogger
    {
        public LoggerBridge()
            : this(LoggerFactory.Create(builder=>
                                       {
                                           builder.AddConsole();
                                       }))
        {
        }

        public LoggerBridge(ILoggerFactory factory)
            : base(factory)
        {
        }

        public void Inform(string message)
        {
            this.LogInformation(message);
        }

        public void Warn(string message)
        {
            this.LogWarning(message);
        }

        public void Error<TExceptionType>(TExceptionType exception, Func<TExceptionType, string> message) 
            where TExceptionType : Exception
        {
            this.LogError(exception, message.Invoke(exception));
        }
    }
}
