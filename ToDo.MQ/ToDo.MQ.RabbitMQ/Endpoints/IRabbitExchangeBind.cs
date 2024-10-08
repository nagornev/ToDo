namespace ToDo.MQ.RabbitMQ.Endpoints
{
    public interface IRabbitExchangeBind
    {
        IRabbitExchange Exchage { get; }

        IRabbitExchange ToExchange { get; }

        IRabbitQueue ToQueue { get; }

        string RoutingKey { get; }

        IReadOnlyDictionary<string, object> Arguments { get; }
    }
}
