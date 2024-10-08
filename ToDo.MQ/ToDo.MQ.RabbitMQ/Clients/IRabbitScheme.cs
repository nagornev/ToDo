using RabbitMQ.Client;
using ToDo.MQ.RabbitMQ.Endpoints;

namespace ToDo.MQ.RabbitMQ.Clients
{
    public interface IRabbitScheme
    {
        IConnection Connection { get; }

        IReadOnlyCollection<IRabbitExchange> Exchanges { get; }

        IReadOnlyCollection<IRabbitQueue> Queues { get; }
    }
}
