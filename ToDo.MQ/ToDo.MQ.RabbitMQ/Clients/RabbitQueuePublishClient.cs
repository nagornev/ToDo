using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ToDo.MQ.Abstractions;
using ToDo.MQ.RabbitMQ.Handlers;

namespace ToDo.MQ.RabbitMQ.Clients
{
    public class RabbitQueuePublishClient : IMessageQueuePublishClient
    {
        private readonly IRabbitScheme _scheme;

        private readonly IReadOnlyCollection<IRabbitPublishHandler> _publishers;

        public RabbitQueuePublishClient(IRabbitScheme scheme,
                                        IReadOnlyCollection<IRabbitPublishHandler> publishers)
        {
            _scheme = scheme;
            _publishers = publishers;
        }

        public Task Publish<TMessageType>(TMessageType message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            using (var channel = _scheme.Connection.CreateModel())
            {
                IEnumerable<IRabbitPublishHandler> publishers = _publishers.Where(x => x.MessageType == typeof(TMessageType));

                if (publishers.Count() < 1)
                    throw new ArgumentException($"No publishers with '{typeof(TMessageType).Name}' type of message.");

                byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                foreach (IRabbitPublishHandler publisher in publishers)
                {
                    IBasicProperties properties = channel.CreateBasicProperties();

                    publisher.Properties?.Invoke(properties);

                    channel.BasicPublish(exchange: publisher.Exchange?.Name ?? string.Empty,
                                         routingKey: publisher.RoutingKey,
                                         basicProperties: properties,
                                         body: body);
                }
            }

            return Task.CompletedTask;
        }
    }
}
