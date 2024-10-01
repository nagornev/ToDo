
using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.Entries.API.Backgrounds
{
    public class MessageQueueBackground : BackgroundService
    {
        private readonly IMessageQueueClient _messageQueue;

        public MessageQueueBackground(IMessageQueueClient messageQueue)
        {
            _messageQueue = messageQueue;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
