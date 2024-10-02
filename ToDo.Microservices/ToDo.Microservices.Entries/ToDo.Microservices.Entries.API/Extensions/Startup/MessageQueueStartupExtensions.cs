using ToDo.Microservices.Entries.API.Backgrounds;
using ToDo.Microservices.Entries.Infrastructure.Consumers;
using ToDo.Microservices.MQ;
using ToDo.Microservices.MQ.Publishers;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class MessageQueueStartupExtensions
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

                    builder.CreaetQueue(DeleteCategoryConsumer.Queue,
                                        true,
                                        false,
                                        false)
                              .AddBind(exchange => exchange.Name == DeleteCategoryPublish.Exchange)
                              .Build();
                });

                configure.AddWorkers(builder =>
                {
                    builder.AddConsumer<NewUserConsumer>(queue => queue.Name == NewUserConsumer.Queue,
                                                         false,
                                                         false);

                    builder.AddConsumer<DeleteCategoryConsumer>(queue => queue.Name == DeleteCategoryConsumer.Queue,
                                                                false,
                                                                false);
                });
            });

            services.AddScoped<NewUserConsumer>();
            services.AddScoped<DeleteCategoryConsumer>();
        }

        private static void AddHostMessageQueue(this IServiceCollection services)
        {
            services.AddHostedService<MessageQueueBackground>();
        }
    }
}
