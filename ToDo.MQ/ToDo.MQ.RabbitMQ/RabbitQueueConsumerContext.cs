using ToDo.MQ.Abstractions;

namespace ToDo.MQ.RabbitMQ
{
    internal class RabbitQueueConsumerContext : IMessageQueueConsumerContext
    {
        private Action _ack;

        public RabbitQueueConsumerContext(byte[] body, Action ack)
        {
            _ack = ack;
            Body = body;
        }

        public byte[] Body { get; private set; }

        public byte[] Response { get; private set; }

        public void Ack()
        {
            _ack.Invoke();
        }

        public void Respond(byte[] response)
        {
            Response = response;
        }

        public void Repeat(int delay = 1000)
        {
            throw new RabbitRepeatException(delay);
        }
    }
}
