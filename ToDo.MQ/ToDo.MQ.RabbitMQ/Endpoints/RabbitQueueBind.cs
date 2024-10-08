namespace ToDo.MQ.RabbitMQ.Endpoints
{
    public class RabbitQueueBind : IRabbitQueueBind   
    {
        public RabbitQueueBind(RabbitQueue queue,
                               RabbitExchange from,
                               string routingKey,
                               Dictionary<string, object> arguments)
        {
            Queue = queue;
            From = from;
            RoutingKey = routingKey;
            Arguments = arguments;
        }

        public IRabbitQueue Queue { get; private set; }

        public IRabbitExchange From { get; private set; }

        public string RoutingKey { get; private set; }

        public IReadOnlyDictionary<string, object> Arguments { get; private set; }
    }
}
