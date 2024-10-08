using RabbitMQ.Client;
using ToDo.MQ.RabbitMQ.Endpoints;

namespace ToDo.MQ.RabbitMQ.Handlers
{
    internal class RabbitProcedureHandler : RabbitPublishHandler, IRabbitProcedureHandler
    {
        public RabbitProcedureHandler(Type messageType,
                                      IRabbitExchange? exchange,
                                      string routingKey,
                                      Action<IBasicProperties> properties)
            : base(messageType, exchange, routingKey, properties)
        {
        }
    }
}
