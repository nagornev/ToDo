namespace ToDo.MQ.Abstractions
{
    public class MessageQueueBuilder
    {
        private IMessageQueueClient _messageQueue;

        private MessageQueueBuilder() 
        {
        }

        public static MessageQueueBuilder Create()
        {
            return new MessageQueueBuilder();
        }

        public MessageQueueBuilder UseMessageQueue(IMessageQueueClient messageQueue)
        {
            _messageQueue = messageQueue;

            return this;
        }

        public IMessageQueueClient Build()
        {
            return _messageQueue;
        }
    }
}
