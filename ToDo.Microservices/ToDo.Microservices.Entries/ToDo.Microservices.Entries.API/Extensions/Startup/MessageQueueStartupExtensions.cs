using ToDo.Microservices.Entries.API.Backgrounds;
using ToDo.Microservices.Entries.Infrastructure.Consumers;
using ToDo.Microservices.MQ;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.RabbitMQ.Endpoints;
using ToDo.Microservices.MQ.Queries.GetCategories;
using ToDo.Microservices.MQ.Queries.GetCategory;

namespace ToDo.Microservices.Entries.API.Extensions.Startup
{
    public static class MessageQueueStartupExtensions
    {
        public static void AddToDoMessageQueue(this IServiceCollection services)
        {
            services.ConfigureMessageQueue();
            services.AddConsumers();
            services.AddConsumeClient();
        }

        private static void ConfigureMessageQueue(this IServiceCollection services)
        {
            services.AddToDoMessageQueue(configure =>
            {
                configure.AddEndpoints(builder =>
                {
                    builder.CreaetQueue(NewUserConsumer.Queue,
                                        true,
                                        false,
                                        false)
                              .AddBind(exchange => exchange.Name == NewUserPublishMessage.Exchange)
                              .Build();

                    builder.CreaetQueue(DeleteCategoryConsumer.Queue,
                                        true,
                                        false,
                                        false)
                              .AddBind(exchange => exchange.Name == DeleteCategoryPublishMessage.Exchange)
                              .Build();
                });

                configure.AddHandlers(builder =>
                {
                    builder.AddConsumer<NewUserConsumer>(queue => queue.Name == NewUserConsumer.Queue,
                                                         false,
                                                         false);

                    builder.AddConsumer<DeleteCategoryConsumer>(queue => queue.Name == DeleteCategoryConsumer.Queue,
                                                                false,
                                                                false);

                    builder.AddProcedure<GetCategoriesProcedureRequest>((IRabbitQueue queue) => queue.Name == GetCategoriesProcedureRequest.Queue,
                                                                  (IRabbitQueue queue) => queue.Name == GetCategoriesProcedureResponse.Queue);

                    builder.AddProcedure<GetCategoryProcedureRequest>((IRabbitQueue queue) => queue.Name == GetCategoryProcedureRequest.Queue,
                                                                (IRabbitQueue queue) => queue.Name == GetCategoryProcedureResponse.Queue);
                });
            });
        }

        private static void AddConsumers(this IServiceCollection services)
        {
            services.AddScoped<NewUserConsumer>();
            services.AddScoped<DeleteCategoryConsumer>();
        }

        private static void AddConsumeClient(this IServiceCollection services)
        {
            services.AddHostedService<MessageQueueConsumeBackground>();
        }
    }
}
