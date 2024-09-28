using RabbitMQ.Client;

namespace ToDo.MQ.RabbitMQ
{
    internal class RabbitEndpointsScheme
    {
        private bool _configured;

        public RabbitEndpointsScheme(IReadOnlyCollection<IRabbitExchange> exchanges,
                                     IReadOnlyCollection<IRabbitQueue> queues) 
        {
            Exchanges = exchanges;
            Queues = queues;
        }

        public IReadOnlyCollection<IRabbitExchange> Exchanges { get; }

        public IReadOnlyCollection<IRabbitQueue> Queues { get; }

        public void Configure(IModel channel)
        {
            if (_configured)
                return;

            foreach (var exchange in Exchanges)
            {
                channel.ExchangeDeclare(exchange: exchange.Name,
                                        type: exchange.Type,
                                        durable: exchange.Durable,
                                        autoDelete: exchange.AutoDelete,
                                        arguments: exchange.Arguments);
            }

            foreach (var queue in Queues)
            {
                channel.QueueDeclare(queue: queue.Name,
                                     durable: queue.Durable,
                                     exclusive: queue.Exclusive,
                                     autoDelete: queue.AutoDelete,
                                     arguments: queue.Arguments);
            }

            foreach (var exchange in Exchanges)
            {
                foreach (var bind in exchange.Binds)
                {
                    if (bind.ToExchange is null)
                        channel.QueueBind(bind.ToQueue.Name, bind.Exchage.Name, bind.RoutingKey, bind.Arguments);
                    else
                        channel.ExchangeBind(bind.ToExchange.Name, bind.Exchage.Name, bind.RoutingKey, bind.Arguments);
                }
            }

            foreach (var queue in Queues)
            {
                foreach (var bind in queue.Binds)
                {
                    channel.QueueBind(bind.Queue.Name, bind.From.Name, bind.RoutingKey, bind.Arguments);
                }
            }

            _configured = true;
        }
    }
}
