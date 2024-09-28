namespace ToDo.MQ.RabbitMQ
{

    public class RabbitExchangeBind
    {
        public RabbitExchangeBind(RabbitExchange exchange,
                                  RabbitExchange to,
                                  string routingKey,
                                  IDictionary<string, object> arguments)
            : this(exchange, routingKey, arguments)
        {
            ToExchange = to;
        }

        public RabbitExchangeBind(RabbitExchange exchange,
                                  RabbitQueue to,
                                  string routingKey,
                                  IDictionary<string, object> arguments)
            : this(exchange, routingKey, arguments)
        {
            ToQueue = to;
        }

        private RabbitExchangeBind(RabbitExchange exchange, string routingKey, IDictionary<string, object> arguments)
        {
            Exchage = exchange;
            RoutingKey = routingKey;
            Arguments = arguments;
        }

        public RabbitExchange Exchage { get; private set; }

        public RabbitExchange ToExchange { get; private set; }

        public RabbitQueue ToQueue { get; private set; }

        public string RoutingKey { get; private set; }

        public IDictionary<string, object> Arguments { get; private set; }
    }
}
