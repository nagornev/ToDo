using MQ.Abstractions;
using ToDo.MQ.RabbitMQ.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using ToDo.MQ.Abstractions;

namespace ToDo.MQ.RabbitMQ
{
    internal class RabbitQueueClient : IMessageQueueClient, IDisposable
    {
        private bool _disposed;

        private ConnectionFactory _factory;

        private IConnection _connection;

        private RabbitEndpointsScheme _endpoints;

        private readonly RabbitWorkerScheme _workers;

        private ConcurrentDictionary<string, RabbitRpc> _rpcs;

        public RabbitQueueClient(ConnectionFactory factory,
                                 RabbitEndpointsScheme endpoints,
                                 RabbitWorkerScheme workers)
        {
            _factory = factory;
            _endpoints = endpoints;
            _workers = workers;
            _rpcs = new ConcurrentDictionary<string, RabbitRpc>();
        }

        #region Connect

        public void Connect()
        {
            if (_connection != null)
                return;

            _connection = _factory.CreateConnection();

            Configure();
            Consume();
        }

        private void Configure()
        {
            using (var channel = _connection.CreateModel())
            {
                _endpoints.Configure(channel);
            }
        }

        private void Consume()
        {
            foreach (var consumer in _workers.Consumers)
            {
                IModel channel = _connection.CreateModel();

                IBasicConsumer basic = CreateConsumer(channel, consumer);

                channel.BasicConsume(consumer.Queue.Name,
                                     consumer.AutoAck,
                                     basic);

                consumer.Channel = channel;
            }
        }

        private IBasicConsumer CreateConsumer(IModel channel, RabbitWorkerConsumer consumer)
        {
            EventingBasicConsumer basic = new EventingBasicConsumer(channel);

            basic.Received += (sender, args) =>
            {
                RabbitQueueConsumerContext context = new RabbitQueueConsumerContext(args.Body.ToArray(),
                                                                                    () => channel.BasicAck(args.DeliveryTag, false));

                consumer.Worker.Consume(context);

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


        #endregion

        #region Pusblish

        public Task Publish<TMessageType>(TMessageType message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            using (var channel = _connection.CreateModel())
            {
                byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                IEnumerable<RabbitWorkerProducer> producers = _workers.Publishers.Where(x => x.MessageType == typeof(TMessageType));

                if (producers.Count() == 0)
                    throw new ArgumentException($"No producers with '{typeof(TMessageType).Name}' type of message.");

                foreach (var producer in producers)
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

        #endregion

        #region RPC

        public Task<IEnumerable<byte>> Send<TRequestType>(TRequestType message, CancellationToken cancellationToken = default)
        {
            IModel channel = _connection.CreateModel();

            IBasicProperties properties = channel.CreateBasicProperties();

            RabbitWorkerProducer? worker = _workers.RPCs.TryGetOne(x => x.MessageType == typeof(TRequestType), out RabbitWorkerProducer? result) ?
                                                 result : throw new ArgumentException("No RPC worker or more than one.");

            worker.Properties.Invoke(properties);

            //Handling response

            var basic = new EventingBasicConsumer(channel);
            basic.Received += (sender, args) =>
            {
                if (!_rpcs.TryRemove(args.BasicProperties.CorrelationId, out var value))
                    return;

                value.Source.TrySetResult(args.Body.ToArray());

                value.Channel.Close();
                value.Channel.Dispose();
            };

            channel.BasicConsume(queue: properties.ReplyTo,
                                 autoAck: true,
                                 consumer: basic);

            //Handling request

            byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            RabbitRpc rpc = new RabbitRpc(channel);

            _rpcs.TryAdd(properties.CorrelationId, rpc);

            channel.BasicPublish(exchange: worker.Exchange?.Name ?? string.Empty,
                                 routingKey: worker.RoutingKey,
                                 basicProperties: properties,
                                 body: body);

            cancellationToken.Register(() => _rpcs.TryRemove(properties.CorrelationId, out _));

            return rpc.Source.Task;
        }

        #endregion

        public void Dispose()
        {
            if (_disposed)
                return;

            _workers.Consumers.ForEach(x => x.Channel.Dispose());
            _connection.Dispose();

            _disposed = true;
        }

     
    }
}
