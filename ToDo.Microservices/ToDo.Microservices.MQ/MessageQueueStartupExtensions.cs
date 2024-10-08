using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.Abstractions;
using ToDo.MQ.RabbitMQ;
using ToDo.MQ.RabbitMQ.Endpoints;
using ToDo.Microservices.MQ.Queries.GetCategories;
using ToDo.Microservices.MQ.Queries.GetCategory;

namespace ToDo.Microservices.MQ
{
    public static class MessageQueueStartupExtensions
    {
        public static void AddMessageQueue(this IServiceCollection services, Action<MessageQueueBuilder> options)
        {
            MessageQueueBuilder builder = MessageQueueBuilder.Create(services);

            options.Invoke(builder);
        }

        public static void AddToDoMessageQueue(this IServiceCollection services, Action<RabbitQueueClientBuilder> configure = null)
        {
            services.AddMessageQueue(options =>
            {
                options.UseRabbit(options =>
                {
                    options.SetConnection(connection =>
                    {
                        connection.HostName = "localhost";
                        connection.UserName = "user";
                        connection.Password = "HHHHhhhh1111";
                    });

                    options.AddEndpoints(builder =>
                    {
                        //publish new user
                        builder.CreateExchange(NewUserPublishMessage.Exchange,
                                               ExchangeType.Fanout,
                                               true,
                                               false)
                                   .Build();


                        //delete category
                        builder.CreateExchange(DeleteCategoryPublishMessage.Exchange,
                                               ExchangeType.Fanout,
                                               true,
                                               false)
                                    .Build();

                        //get categories rpc
                        builder.CreaetQueue(GetCategoriesProcedureRequest.Queue,
                                            false,
                                            false,
                                            false)
                                   .Build()
                               .CreaetQueue(GetCategoriesProcedureResponse.Queue,
                                            false,
                                            false,
                                            false)
                                   .Build();


                        //get category rpc
                        builder.CreaetQueue(GetCategoryProcedureRequest.Queue,
                                            false,
                                            false,
                                            false)
                                   .Build()
                               .CreaetQueue(GetCategoryProcedureResponse.Queue,
                                            false,
                                            false,
                                            false)
                                   .Build();
                    });

                    configure?.Invoke(options);
                });
            });
        }
    }
}
