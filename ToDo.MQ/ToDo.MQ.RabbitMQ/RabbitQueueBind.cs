namespace ToDo.MQ.RabbitMQ
{
    public class RabbitQueueBind
    {
        public RabbitQueueBind(RabbitQueue queue,
                               RabbitExchange from,
                               string routingKey,
                               IDictionary<string, object> arguments)
        {
            Queue = queue;
            From = from;
            RoutingKey = routingKey;
            Arguments = arguments;
        }

        public RabbitQueue Queue { get; private set; }

        public RabbitExchange From { get; private set; }

        public string RoutingKey { get; private set; }

        public IDictionary<string, object> Arguments { get; private set; }
    }
}
