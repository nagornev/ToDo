namespace ToDo.MQ.RabbitMQ.Endpoints
{
    public interface IRabbitExchange
    {
        string Name { get; }

        string Type { get; }

        bool Durable { get; }

        bool AutoDelete { get; }

        IDictionary<string, object> Arguments { get; }

        IReadOnlyCollection<IRabbitExchangeBind> Binds { get; }
    }
}
