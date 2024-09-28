namespace ToDo.MQ.RabbitMQ
{
    public class RabbitEndpointExchangeBuilder
    {
        private RabbitEndpointsBuilder _head;

        private RabbitExchange _exchange;

        private IReadOnlyCollection<RabbitExchange> _exchanges;

        private IReadOnlyCollection<RabbitQueue> _queues;

        internal RabbitEndpointExchangeBuilder(RabbitEndpointsBuilder head,
                                               RabbitExchange created,
                                               IReadOnlyCollection<RabbitExchange> exchanges,
                                               IReadOnlyCollection<RabbitQueue> queues)
        {
            _head = head;
            _exchange = created;
            _exchanges = exchanges;
            _queues = queues;
        }

        public RabbitEndpointExchangeBuilder AddBind(Func<IRabbitExchange, bool> toPredicate,
                                                     string routingKey = "",
                                                     IDictionary<string, object> arguments = null)
        {
            RabbitExchange to = _exchanges.First(toPredicate.Invoke);

            _exchange.AddBind(new RabbitExchangeBind(_exchange, to, routingKey, arguments));
            to.AddBind(new RabbitExchangeBind(_exchange, to, routingKey, arguments));

            return this;
        }

        public RabbitEndpointExchangeBuilder AddBind(Func<IRabbitQueue, bool> toPredicate,
                                                     string routingKey = "",
                                                     IDictionary<string, object> arguments = null)
        {
            RabbitQueue to = _queues.First(toPredicate.Invoke);

            _exchange.AddBind(new RabbitExchangeBind(_exchange, to, routingKey, arguments));
            to.AddBind(new RabbitQueueBind(to, _exchange, routingKey, arguments));

            return this;
        }


        public RabbitEndpointsBuilder Build()
        {
            return _head;
        }
    }
}
