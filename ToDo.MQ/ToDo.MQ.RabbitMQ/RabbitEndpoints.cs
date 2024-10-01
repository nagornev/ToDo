namespace ToDo.MQ.RabbitMQ
{
    internal class RabbitEndpoints
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
