using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using ToDo.MQ.Abstractions;
using ToDo.MQ.RabbitMQ.Extensions;
using ToDo.MQ.RabbitMQ.Handlers;

namespace ToDo.MQ.RabbitMQ.Clients
{
    internal class RabbitQueueProcedureClient : IMessageQueueProcedureClient
    {
        private class ProcedureTask
        {
            private readonly int _timeout;

            public ProcedureTask(int timeout = 5000)
            {
                _timeout = timeout;
                Source = CreateSource();

                CreateSource();
            }

            public TaskCompletionSource<IEnumerable<byte>> Source { get; private set; }

            private TaskCompletionSource<IEnumerable<byte>> CreateSource()
            {
                TaskCompletionSource<IEnumerable<byte>> source = new TaskCompletionSource<IEnumerable<byte>>();

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += (sender, args) =>
                {
                    if (!source.Task.IsCompleted)
                    {
                        source.SetException(new TimeoutException());
                    }
                };
                timer.AutoReset = false;
                timer.Enabled = false;
                timer.Interval = _timeout;

                timer.Start();

                return source;
            }
        }

        private readonly IRabbitScheme _scheme;

        private readonly IReadOnlyCollection<IRabbitProcedureHandler> _procedures;

        private readonly IModel _channel;

        private readonly ConcurrentDictionary<string, ProcedureTask> _rpcs;

        public RabbitQueueProcedureClient(IRabbitScheme scheme,
                                          IReadOnlyCollection<IRabbitProcedureHandler> procedures)
        {
            _scheme = scheme;
            _procedures = procedures;
            _channel = Configure();
            _rpcs = new ConcurrentDictionary<string, ProcedureTask>();
        }

        private IModel Configure()
        {
            IModel channel = _scheme.Connection.CreateModel();

            foreach (RabbitProcedureHandler procedurer in _procedures)
            {
                IBasicProperties properties = channel.CreateBasicProperties();
                procedurer.Properties(properties);

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, args) =>
                {
                    if (!_rpcs.TryRemove(args.BasicProperties.CorrelationId, out var value))
                        return;

                    value.Source.TrySetResult(args.Body.ToArray());
                };

                channel.BasicConsume(queue: properties.ReplyTo,
                                     autoAck: true,
                                     consumer: consumer);
            }

            return channel;
        }

        public Task<IEnumerable<byte>> Send<TRequestType>(TRequestType request, CancellationToken cancellationToken = default)
        {
            IRabbitProcedureHandler procedure = GetProcedure<TRequestType>();

            IBasicProperties properties = GetProperties(procedure);

            byte[] body = GetBody(request, Encoding.UTF8);

            if (!_rpcs.TryAdd(properties.CorrelationId, new ProcedureTask(), out ProcedureTask procedureTask))
                throw new InvalidOperationException("The procedure task can not be added.");

            _channel.BasicPublish(exchange: procedure.Exchange?.Name ?? string.Empty,
                                  routingKey: procedure.RoutingKey,
                                  basicProperties: properties,
                                  body: body);

            cancellationToken.Register(() => _rpcs.TryRemove(properties.CorrelationId, out _));

            return procedureTask.Source.Task;
        }

        private IRabbitProcedureHandler GetProcedure<TRequestType>()
        {
            return _procedures.TryGetOne(x => x.MessageType == typeof(TRequestType), out var result) ?
                      result! :
                      throw new ArgumentException($"No procedures or more than one with '{typeof(TRequestType).Name}' type of request.");
        }

        private IBasicProperties GetProperties(IRabbitProcedureHandler procedure)
        {
            IBasicProperties properties = _channel.CreateBasicProperties();
            procedure.Properties(properties);

            return properties;
        }

        private byte[] GetBody<T>(T message, Encoding encoding)
        {
            return encoding.GetBytes(JsonSerializer.Serialize(message));
        }
    }
}
