using ToDo.Microservices.Identity.API.Backgrounds;
using ToDo.Microservices.Identity.Infrastructure.Publishers;
using ToDo.Microservices.Identity.UseCases.Publishers;
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
            services.AddScoped<IUserPublisher, UserPublisher>();
        }

        private static void AddHostMessageQueue(this IServiceCollection services)
        {
            services.AddHostedService<MessageQueueBackground>();
        }
    }
}
