using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ToDo.MQ.Abstractions;
using ToDo.MQ.RabbitMQ.Handlers;

namespace ToDo.MQ.RabbitMQ.Clients
{
    public class RabbitQueueConsumeClient : IMessageQueueConsumeClient
    {
        private readonly IServiceProvider _provider;

        private readonly IRabbitScheme _scheme;

        private readonly IReadOnlyCollection<IRabbitConsumeHandler> _consumers;

        private readonly IDictionary<IRabbitConsumeHandler, IModel> _channels;

        public RabbitQueueConsumeClient(IServiceProvider provider,
                                        IRabbitScheme scheme,
                                        IReadOnlyCollection<IRabbitConsumeHandler> consumers)
        {
            _provider = provider;
            _scheme = scheme;
            _consumers = consumers;
            _channels = new Dictionary<IRabbitConsumeHandler, IModel>();
        }

        public Task Start()
        {
            foreach (IRabbitConsumeHandler consumer in _consumers)
            {
                IModel channel = _scheme.Connection.CreateModel();

                IBasicConsumer basic = CreateBasic(consumer, channel);

                channel.BasicConsume(consumer.Queue.Name,
                                     consumer.AutoAck,
                                     basic);

                _channels.Add(consumer, channel);
            }

            return Task.CompletedTask;
        }

        private IBasicConsumer CreateBasic(IRabbitConsumeHandler consumer, IModel channel)
        {
            EventingBasicConsumer basic = new EventingBasicConsumer(channel);

            basic.Received += async (sender, args) =>
            {
                RabbitQueueConsumerContext context = new RabbitQueueConsumerContext(args.Body.ToArray(),
                                                                                    () => channel.BasicAck(args.DeliveryTag, false));

                do
                {
                    try
                    {
                        using (var scope = _provider.CreateScope())
                        {
                            IMessageQueueConsumer handler = (IMessageQueueConsumer)scope.ServiceProvider.GetRequiredService(consumer.Handler);
                            await handler.Consume(context);
                        }

                        break;
                    }
                    catch (RabbitRepeatException exception)
                    {
                        await Task.Delay(exception.Delay);
                        continue;
                    }
                }
                while (true);

                if (consumer.Reply)
                {
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.CorrelationId = args.BasicProperties.CorrelationId;

                    channel.BasicPublish(exchange: string.Empty,
                                         routingKey: args.BasicProperties.ReplyTo,
                                         basicProperties: properties,
                                         body: context.Response);
                }
            };

            return basic;
        }

        public Task Stop()
        {
            foreach (IRabbitConsumeHandler consumer in _consumers)
            {
                if (_channels.TryGetValue(consumer, out var channel))
                {
                    channel.Close();

                    _channels.Remove(consumer);
                }
            }

            return Task.CompletedTask;
        }
    }
}
