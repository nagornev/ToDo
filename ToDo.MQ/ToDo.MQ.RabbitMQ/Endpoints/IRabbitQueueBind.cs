namespace ToDo.MQ.RabbitMQ.Endpoints
{
    public interface IRabbitQueueBind
    {
        IRabbitQueue Queue { get; }

        IRabbitExchange From { get; }

        string RoutingKey { get; }

        IReadOnlyDictionary<string, object> Arguments { get; }
    }
}
