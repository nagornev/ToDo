using RabbitMQ.Client;

namespace ToDo.MQ.RabbitMQ
{
    public class RabbitWorkerConsumer
    {
        public RabbitWorkerConsumer(IRabbitQueue queue,
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

        internal IModel Channel { get; set; }
    }
}
