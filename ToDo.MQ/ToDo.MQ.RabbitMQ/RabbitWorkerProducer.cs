using RabbitMQ.Client;

namespace ToDo.MQ.RabbitMQ
{
    public class RabbitWorkerProducer
    {
        public RabbitWorkerProducer(Type messageType,
                                    IRabbitExchange? exchange,
                                    string routingKey,
                                    Action<IBasicProperties> properties)
        {
            MessageType = messageType;
            Exchange = exchange;
            RoutingKey = routingKey;
            Properties = properties;
        }

        public Type MessageType { get; private set; }

        public IRabbitExchange? Exchange { get; private set; }

        public string RoutingKey { get; private set; }

        public Action<IBasicProperties> Properties { get; private set; }
    }
}
