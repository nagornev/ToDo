using RabbitMQ.Client;

namespace ToDo.MQ.RabbitMQ
{
    public class RabbitEndpointsBuilder
    {
        private List<RabbitExchange> _exchanges;

        private List<RabbitQueue> _queues;

        public RabbitEndpointsBuilder()
        {
            _exchanges = new List<RabbitExchange>();
            _queues = new List<RabbitQueue>();
        }

        public RabbitEndpointExchangeBuilder CreateExchange(string name,
                                                            string type = ExchangeType.Fanout,
                                                            bool durable = false,
                                                            bool autoDelete = false,
                                                            IDictionary<string, object> arguments = null)
        {
            if (_exchanges.Any(x => x.Name == name))
                throw new ArgumentException($"The exchange with '{name}' name is already added.");


            RabbitExchange exchange = new RabbitExchange(name,
                                                         type,
                                                         durable,
                                                         autoDelete,
                                                         arguments);

            _exchanges.Add(exchange);

            return new RabbitEndpointExchangeBuilder(this, exchange, _exchanges, _queues);
        }

        public RabbitEndpointQueueBuilder CreaetQueue(string name,
                                                      bool durable = true,
                                                      bool exclusive = false,
                                                      bool autoDelete = false,
                                                      IDictionary<string, object> arguments = null)
        {
            if (_queues.Any(x => x.Name == name))
                throw new ArgumentException($"The queue with '{name}' name already added.");

            RabbitQueue queue = new RabbitQueue(name,
                                                durable,
                                                exclusive,
                                                autoDelete,
                                                arguments);

            _queues.Add(queue);

            return new RabbitEndpointQueueBuilder(this, queue, _exchanges);
        }

        internal RabbitEndpointsScheme Build()
        {
            return new RabbitEndpointsScheme(_exchanges, _queues);
        }
    }
}

