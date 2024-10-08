namespace ToDo.MQ.RabbitMQ.Endpoints
{
    public interface IRabbitEndpoints
    {
        IReadOnlyCollection<IRabbitExchange> Exchanges { get; }

        IReadOnlyCollection<IRabbitQueue> Queues { get; }
    }
}
