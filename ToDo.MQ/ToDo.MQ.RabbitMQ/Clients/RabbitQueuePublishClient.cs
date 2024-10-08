using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
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
                byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                IEnumerable<IRabbitPublishHandler> publishers = _publishers.Where(x => x.MessageType == typeof(TMessageType));

                if (publishers.Count() == 0)
                    throw new ArgumentException($"No publishers with '{typeof(TMessageType).Name}' type of message.");

                foreach (var producer in publishers)
                {
                    IBasicProperties properties = channel.CreateBasicProperties();

                    producer.Properties?.Invoke(properties);

                    channel.BasicPublish(exchange: producer.Exchange?.Name ?? string.Empty,
                                         routingKey: producer.RoutingKey,
                                         basicProperties: properties,
                                         body: body);
                }
            }

            return Task.CompletedTask;
        }
    }
}
