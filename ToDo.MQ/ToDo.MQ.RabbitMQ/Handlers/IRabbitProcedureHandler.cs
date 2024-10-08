﻿using RabbitMQ.Client;
using ToDo.MQ.RabbitMQ.Endpoints;

namespace ToDo.MQ.RabbitMQ.Handlers
{
    public interface IRabbitProcedureHandler
    {
        Type MessageType { get; }

        IRabbitExchange? Exchange { get; }

        string RoutingKey { get; }

        Action<IBasicProperties> Properties { get; }
    }
}
