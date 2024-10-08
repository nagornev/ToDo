namespace ToDo.Microservices.Middleware.Exceptions
{
    public class GlobalExceptionHandlerConfigurationBuilder
    {
        private class GlobalExceptionHandlerConfiguration : IGlobalExceptionHandlerConfiguration
        {
            public GlobalExceptionHandlerConfiguration(string serviceName)
            {
                ServiceName = serviceName;
            }

            public string ServiceName { get; private set; }
        }

        public string ServiceName { get; set; }

        internal IGlobalExceptionHandlerConfiguration Build()
        {
            return new GlobalExceptionHandlerConfiguration(ServiceName);
        }
    }
}
