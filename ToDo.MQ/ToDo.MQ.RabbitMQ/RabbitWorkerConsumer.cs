using MQ.Abstractions;
using RabbitMQ.Client;
using ToDo.MQ.Abstractions;

namespace ToDo.MQ.RabbitMQ
{
    public class RabbitWorkerConsumer
    {
        public RabbitWorkerConsumer(IRabbitQueue queue,
                                    IMessageQueueConsumer worker,
                                    bool autoAck,
                                    bool reply)
        {
            Queue = queue;
            AutoAck = autoAck;
            Worker = worker;
            Reply = reply;
        }

        public IRabbitQueue Queue { get; private set; }

        public bool AutoAck { get; private set; }

        public IMessageQueueConsumer Worker { get; private set; }
        
        public bool Reply { get; private set; }

        internal IModel Channel { get; set; }
    }
}
