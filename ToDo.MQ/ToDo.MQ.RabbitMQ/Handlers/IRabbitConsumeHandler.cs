using ToDo.MQ.RabbitMQ.Endpoints;

namespace ToDo.MQ.RabbitMQ.Handlers
{
    public interface IRabbitConsumeHandler
    {
        IRabbitQueue Queue { get; }

        Type Handler { get; }

        bool AutoAck { get; }

        bool Reply { get; }
    }
}
