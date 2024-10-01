namespace ToDo.MQ.RabbitMQ
{
    public interface IRabbitQueue
    {

        string Name { get; }

        bool Durable { get; }

        bool Exclusive { get; }

        bool AutoDelete { get; }

        IDictionary<string, object> Arguments { get; }

        IReadOnlyCollection<RabbitQueueBind> Binds { get; }

    }
}
