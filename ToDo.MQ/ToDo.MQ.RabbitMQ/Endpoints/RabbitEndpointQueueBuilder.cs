namespace ToDo.MQ.RabbitMQ.Endpoints
{
    public class RabbitEndpointQueueBuilder
    {
        private RabbitEndpointsBuilder _head;

        private RabbitQueue _queue;

        private IReadOnlyCollection<RabbitExchange> _exchanges;

        internal RabbitEndpointQueueBuilder(RabbitEndpointsBuilder head,
                                            RabbitQueue created,
                                            IReadOnlyCollection<RabbitExchange> exchanges)
        {
            _head = head;
            _queue = created;
            _exchanges = exchanges;
        }

        public RabbitEndpointQueueBuilder AddBind(Func<IRabbitExchange, bool> fromPredicate,
                                                  string routingKey = "",
                                                  Dictionary<string, object> arguments = null)
        {
            RabbitExchange from = _exchanges.First(fromPredicate.Invoke);

            _queue.AddBind(new RabbitQueueBind(_queue, from, routingKey, arguments));
            from.AddBind(new RabbitExchangeBind(from, _queue, routingKey, arguments));

            return this;
        }

        public RabbitEndpointsBuilder Build()
        {
            return _head;
        }
    }
}
