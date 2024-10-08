namespace ToDo.MQ.RabbitMQ.Endpoints
{

    public class RabbitExchangeBind : IRabbitExchangeBind
    {
        public RabbitExchangeBind(RabbitExchange exchange,
                                  RabbitExchange to,
                                  string routingKey,
                                  Dictionary<string, object> arguments)
            : this(exchange, routingKey, arguments)
        {
            ToExchange = to;
        }

        public RabbitExchangeBind(RabbitExchange exchange,
                                  RabbitQueue to,
                                  string routingKey,
                                  Dictionary<string, object> arguments)
            : this(exchange, routingKey, arguments)
        {
            ToQueue = to;
        }

        private RabbitExchangeBind(RabbitExchange exchange,
                                   string routingKey,
                                   Dictionary<string, object> arguments)
        {
            Exchage = exchange;
            RoutingKey = routingKey;
            Arguments = arguments;
        }

        public IRabbitExchange Exchage { get; private set; }

        public IRabbitExchange ToExchange { get; private set; }

        public IRabbitQueue ToQueue { get; private set; }

        public string RoutingKey { get; private set; }

        public IReadOnlyDictionary<string, object> Arguments { get; private set; }
    }
}
