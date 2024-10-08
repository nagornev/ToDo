using ToDo.Microservices.Identity.API.Backgrounds;
using ToDo.Microservices.Identity.Infrastructure.Publishers;
using ToDo.Microservices.Identity.UseCases.Publishers;
using ToDo.Microservices.MQ;
using ToDo.Microservices.MQ.Publishers;

namespace ToDo.Microservices.Identity.API.Extensions.Startup
{
    public static class MessageQueueStartupExtensions
    {
        public static void AddToDoMessageQueue(this IServiceCollection services)
        {
            services.ConfigureMessageQueue();
            services.AddPublishers();
            services.AddConsumeClient();
        }

        private static void ConfigureMessageQueue(this IServiceCollection services)
        {
            services.AddToDoMessageQueue(configure =>
            {
                configure.AddHandlers(builder =>
                {
                    builder.AddPublisher<NewUserPublishMessage>(exchange => exchange.Name == NewUserPublishMessage.Exchange);
                });
            });
        }

        private static void AddPublishers(this IServiceCollection services)
        {
            services.AddScoped<IUserPublisher, UserPublisher>();
        }

        private static void AddConsumeClient(this IServiceCollection services)
        {
            services.AddHostedService<MessageQueueConsumeBackground>();
        }
    }
}
