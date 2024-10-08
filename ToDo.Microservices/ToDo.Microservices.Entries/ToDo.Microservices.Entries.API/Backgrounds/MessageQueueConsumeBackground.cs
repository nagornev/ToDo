
using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.Entries.API.Backgrounds
{
    public class MessageQueueConsumeBackground : BackgroundService
    {
        private readonly IMessageQueueConsumeClient _consumeClient;

        public MessageQueueConsumeBackground(IMessageQueueConsumeClient consumeClient)
        {
            _consumeClient = consumeClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumeClient.Start();
        }
    }
}
