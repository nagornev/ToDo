using ToDo.Microservices.Identity.API.Backgrounds;
using ToDo.Microservices.Identity.Infrastructure.Producers;
using ToDo.Microservices.Identity.UseCases.Producers;
using ToDo.Microservices.MQ;

namespace ToDo.Microservices.Identity.API.Extensions.Startup
{
    public static class MessageQueueStartupExtensions
    {
        public static void AddToDoMessageQueue(this IServiceCollection services)
        {
            services.AddToDoMessageQueue(configure => { });
            services.AddPublishers();
            services.AddHostMessageQueue();
        }

        private static void AddPublishers(this IServiceCollection services)
        {
            services.AddScoped<IUserProducer, UserProducer>();
        }

        private static void AddHostMessageQueue(this IServiceCollection services)
        {
            services.AddHostedService<MessageQueueBackground>();
        }
    }
}
