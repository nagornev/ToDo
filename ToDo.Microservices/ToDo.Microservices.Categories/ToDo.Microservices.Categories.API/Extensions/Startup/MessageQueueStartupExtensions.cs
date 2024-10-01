using ToDo.Microservices.Categories.API.Backgrounds;
using ToDo.Microservices.Categories.Infrastructure.Consumers;
using ToDo.Microservices.MQ;
using ToDo.Microservices.MQ.Publishers;
using ToDo.Microservices.MQ.RPCs.GetCategories;
using ToDo.Microservices.MQ.RPCs.GetCategory;

namespace ToDo.Microservices.Categories.API.Extensions.Startup
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
                });

                configure.AddWorkers(builder =>
                {
                    builder.AddConsumer<NewUserConsumer>(queue => queue.Name == NewUserConsumer.Queue,
                                                         false,
                                                         false);

                    builder.AddConsumer<GetCategoriesConsumer>(queue => queue.Name == GetCategoriesRpcRequest.Queue,
                                                               false,
                                                               true);

                    builder.AddConsumer<GetCategoryConsumer>(queue => queue.Name == GetCategoryRpcRequest.Queue,
                                                             false,
                                                             true);
                });
            });

            services.AddScoped<NewUserConsumer>();
            services.AddScoped<GetCategoriesConsumer>();
            services.AddScoped<GetCategoryConsumer>();
        }

        private static void AddHostMessageQueue(this IServiceCollection services)
        {
            services.AddHostedService<MessageQueueBackground>();
        }
    }
}
