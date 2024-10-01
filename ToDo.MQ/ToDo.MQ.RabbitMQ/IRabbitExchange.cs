namespace ToDo.MQ.RabbitMQ
{
    public interface IRabbitExchange
    {
        string Name { get; }

        string Type { get; }

        bool Durable { get; }

        bool AutoDelete { get; }

        IDictionary<string, object> Arguments { get; }

        IReadOnlyCollection<RabbitExchangeBind> Binds { get; }
    }
}
