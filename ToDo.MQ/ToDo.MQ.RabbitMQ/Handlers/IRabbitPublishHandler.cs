using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.MQ.RabbitMQ.Endpoints;

namespace ToDo.MQ.RabbitMQ.Handlers
{
    public interface IRabbitPublishHandler
    {
        Type MessageType { get; }

        IRabbitExchange? Exchange { get; }

        string RoutingKey { get; }

        Action<IBasicProperties> Properties { get; }
    }
}
