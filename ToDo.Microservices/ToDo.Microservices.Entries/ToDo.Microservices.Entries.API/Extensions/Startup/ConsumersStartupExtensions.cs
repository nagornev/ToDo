using ToDo.Microservices.Entries.API.Backgrounds;
using ToDo.Microservices.Entries.Infrastructure.Consumers;
using ToDo.Microservices.MQ;
using ToDo.Microservices.MQ.Publishers;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class ConsumersStartupExtensions
    {
        public static void AddToDoMessageQueue(this IServiceCollection services)
        {
            services.AddConsumers();
            services.AddHostMessageQueue();
        }

        private static void AddConsumers(this IServiceCollection services)
        {
            services.AddToDoMessageQueue(configure =>
            {
                configure.AddEndpoints(builder =>
                {
                    builder.CreaetQueue(NewUserConsumer.Queue,
                                        true,
                                        false,
                                        false)
                              .AddBind(exchange => exchange.Name == NewUserPublish.Exchange)
                              .Build();
                });

                configure.AddWorkers(builder =>
                {
                    builder.AddConsumer<NewUserConsumer>(queue => queue.Name == NewUserConsumer.Queue,
                                                         false,
                                                         false);
                });
            });

            services.AddScoped<NewUserConsumer>();
        }

        private static void AddHostMessageQueue(this IServiceCollection services)
        {
            services.AddHostedService<MessageQueueBackground>();
        }
    }
}
