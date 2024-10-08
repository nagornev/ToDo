using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.MQ
{
    public abstract class MessageQueueStableConsumer : IMessageQueueConsumer
    {
        private readonly IReadOnlyDictionary<bool, Action<IMessageQueueConsumerContext>> _completions = new Dictionary<bool, Action<IMessageQueueConsumerContext>>()
        {
            { true, (context)=> context.Ack() },
            { false, (context)=> context.Repeat() },
        };

        public async Task Consume(IMessageQueueConsumerContext context)
        {
            try
            {
                await Execute(context);
            }
            catch
            {
                context.Repeat();
            }
        }

        public abstract Task Execute(IMessageQueueConsumerContext context);

        public void Complete(bool state, IMessageQueueConsumerContext context)
        {
            Action<IMessageQueueConsumerContext> completion = _completions[state];

            completion.Invoke(context);
        }
    }
}
