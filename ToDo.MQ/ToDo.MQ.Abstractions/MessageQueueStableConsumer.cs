using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDo.MQ.Abstractions
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

        protected abstract Task Execute(IMessageQueueConsumerContext context);

        public void Complete(bool state, IMessageQueueConsumerContext context)
        {
            Action<IMessageQueueConsumerContext> completion = _completions[state];

            completion.Invoke(context);
        }
    }
}
