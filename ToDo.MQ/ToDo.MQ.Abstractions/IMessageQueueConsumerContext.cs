namespace ToDo.MQ.Abstractions
{
    public interface IMessageQueueConsumerContext
    {
        byte[] Body { get; }

        void Ack();

        void Respond(byte[] response);

        void Repeat(int delay = 1000);
    }
}
