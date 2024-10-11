using ToDo.Microservices.Categories.API.Backgrounds;
using ToDo.Microservices.Categories.Infrastructure.Consumers;
using ToDo.Microservices.Categories.Infrastructure.Publishers;
using ToDo.Microservices.Categories.UseCases.Publishers;
using ToDo.Microservices.MQ;
using ToDo.Microservices.MQ.Publishers;
using ToDo.Microservices.MQ.Queries.GetCategories;
using ToDo.Microservices.MQ.Queries.GetCategory;

namespace ToDo.Microservices.Categories.API.Extensions.Startup
{
    public static class MessageQueueStartupExtensions
    {
        public static void AddMessageQueue(this IServiceCollection services)
        {
            services.ConfigureMessageQueue();
            services.AddPublishers();
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
                });

                configure.AddHandlers(builder =>
                {
                    builder.AddPublisher<DeleteCategoryPublishMessage>(exchange => exchange.Name == DeleteCategoryPublishMessage.Exchange);

                    builder.AddConsumer<NewUserConsumer>(queue => queue.Name == NewUserConsumer.Queue,
                                                         false,
                                                         false);

                    builder.AddConsumer<GetCategoriesConsumer>(queue => queue.Name == GetCategoriesProcedureRequest.Queue,
                                                               false,
                                                               true);

                    builder.AddConsumer<GetCategoryConsumer>(queue => queue.Name == GetCategoryProcedureRequest.Queue,
                                                             false,
                                                             true);
                });
            });
        }

        private static void AddPublishers(this IServiceCollection services)
        {
            services.AddScoped<ICategoryPubliser, CategoryPublisher>();
        }

        private static void AddConsumers(this IServiceCollection services)
        {
            services.AddScoped<NewUserConsumer>();
            services.AddScoped<GetCategoriesConsumer>();
            services.AddScoped<GetCategoryConsumer>();
        }

        private static void AddConsumeClient(this IServiceCollection services)
        {
            services.AddHostedService<MessageQueueConsumeBackground>();
        }
    }
}
