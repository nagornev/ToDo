namespace ToDo.MQ.Abstractions
{
    public interface IMessageQueueConsumerContext
    {
        byte[] Body { get; }

        void Ack();

        void Respond(byte[] response);
    }
}
