namespace ToDo.MQ.RabbitMQ.Endpoints
{
    internal class RabbitEndpoints : IRabbitEndpoints
    {
        public RabbitEndpoints(IReadOnlyCollection<IRabbitExchange> exchanges,
                               IReadOnlyCollection<IRabbitQueue> queues)
        {
            Exchanges = exchanges;
            Queues = queues;
        }

        public IReadOnlyCollection<IRabbitExchange> Exchanges { get; }

        public IReadOnlyCollection<IRabbitQueue> Queues { get; }
    }
}
