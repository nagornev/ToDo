using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using ToDo.Microservices.MQ.Publishers;
using ToDo.Microservices.MQ.RPCs.GetCategories;
using ToDo.Microservices.MQ.RPCs.GetCategory;
using ToDo.MQ.Abstractions;
using ToDo.MQ.RabbitMQ;

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
                    options.UseConnection(connection =>
                    {
                        connection.HostName = "localhost";
                        connection.UserName = "user";
                        connection.Password = "HHHHhhhh1111";
                    });

                    options.AddEndpoints(builder =>
                    {
                        //publish new user
                        builder.CreateExchange(NewUserPublish.Exchange,
                                               ExchangeType.Fanout,
                                               true,
                                               false)
                                   .Build();


                        //delete category
                        builder.CreateExchange(DeleteCategoryPublish.Exchange,
                                               ExchangeType.Fanout,
                                               true,
                                               false)
                                    .Build();

                        //get categories rpc
                        builder.CreaetQueue(GetCategoriesRpcRequest.Queue,
                                            false,
                                            false,
                                            false)
                                   .Build()
                               .CreaetQueue(GetCategoriesRpcResponse.Queue,
                                            false,
                                            false,
                                            false)
                                   .Build();


                        //get category rpc
                        builder.CreaetQueue(GetCategoryRpcRequest.Queue,
                                            false,
                                            false,
                                            false)
                                   .Build()
                               .CreaetQueue(GetCategoryRpcResponse.Queue,
                                            false,
                                            false,
                                            false)
                                   .Build();
                    });

                    options.AddWorkers(builder =>
                    {
                        builder.AddPublisher<NewUserPublish>(exchange => exchange.Name == NewUserPublish.Exchange);

                        builder.AddPublisher<DeleteCategoryPublish>(exchange => exchange.Name == DeleteCategoryPublish.Exchange);

                        builder.AddRPC<GetCategoriesRpcRequest>((IRabbitQueue queue) => queue.Name == GetCategoriesRpcRequest.Queue,
                                                                (IRabbitQueue queue) => queue.Name == GetCategoriesRpcResponse.Queue);

                        builder.AddRPC<GetCategoryRpcRequest>((IRabbitQueue queue) => queue.Name == GetCategoryRpcRequest.Queue,
                                                              (IRabbitQueue queue) => queue.Name == GetCategoryRpcResponse.Queue);
                    });

                    configure?.Invoke(options);
                });
            });
        }
    }
}
