namespace ToDo.MQ.RabbitMQ.Endpoints
{
    public interface IRabbitQueue
    {

        string Name { get; }

        bool Durable { get; }

        bool Exclusive { get; }

        bool AutoDelete { get; }

        IDictionary<string, object> Arguments { get; }

        IReadOnlyCollection<IRabbitQueueBind> Binds { get; }

    }
}
