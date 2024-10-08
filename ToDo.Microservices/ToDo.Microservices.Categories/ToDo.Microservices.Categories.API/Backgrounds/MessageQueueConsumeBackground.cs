using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.Categories.API.Backgrounds
{
    public class MessageQueueConsumeBackground : BackgroundService
    {
        private readonly IMessageQueueConsumeClient _consumeClient;

        public MessageQueueConsumeBackground(IMessageQueueConsumeClient consumerClient)
        {
            _consumeClient = consumerClient;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumeClient.Start();
        }
    }
}
