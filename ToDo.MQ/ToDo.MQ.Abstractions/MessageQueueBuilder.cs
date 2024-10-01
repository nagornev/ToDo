using Microsoft.Extensions.DependencyInjection;

namespace ToDo.MQ.Abstractions
{
    public class MessageQueueBuilder
    {
        private IServiceCollection _services;

        private MessageQueueBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public static MessageQueueBuilder Create(IServiceCollection services)
        {
            return new MessageQueueBuilder(services);
        }

        public MessageQueueBuilder UseMessageQueue<TMessageQueueClient>()
            where TMessageQueueClient : class, IMessageQueueClient
        {
            _services.AddSingleton<IMessageQueueClient, TMessageQueueClient>();

            return this;
        }

        public IServiceCollection GetServices()
        {
            return _services;
        }
    }
}
