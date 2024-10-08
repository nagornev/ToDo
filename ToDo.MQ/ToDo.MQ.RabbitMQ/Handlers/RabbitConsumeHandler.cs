using RabbitMQ.Client;
using ToDo.MQ.RabbitMQ.Endpoints;

namespace ToDo.MQ.RabbitMQ.Handlers
{
    internal class RabbitConsumeHandler : IRabbitConsumeHandler
    {
        public RabbitConsumeHandler(IRabbitQueue queue,
                                    Type handler,
                                    bool autoAck,
                                    bool reply)
        {
            Queue = queue;
            Handler = handler;
            AutoAck = autoAck;
            Reply = reply;
        }

        public IRabbitQueue Queue { get; private set; }

        public Type Handler { get; private set; }

        public bool AutoAck { get; private set; }

        public bool Reply { get; private set; }
    }
}
