namespace ToDo.Microservices.Middleware.Exceptions
{
    public interface IGlobalExceptionHandlerConfiguration
    {
        string ServiceName { get; }
    }
}
