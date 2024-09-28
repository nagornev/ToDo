using MQ.Abstractions;
using RabbitMQ.Client;
using ToDo.MQ.Abstractions;

namespace ToDo.MQ.RabbitMQ
{
    public class RabbitQueueClientBuilder
    {
        private ConnectionFactory _connectionFactory;

        private RabbitEndpointsBuilder _endpointsBuilder;

        private RabbitWorkersBuildler _workersBuilder;

        private RabbitQueueClientBuilder()
        {
            _connectionFactory = new ConnectionFactory();
            _endpointsBuilder = new RabbitEndpointsBuilder();
            _workersBuilder = new RabbitWorkersBuildler(() => _endpointsBuilder.Build());
        }

        internal static RabbitQueueClientBuilder Create()
        {
            return new RabbitQueueClientBuilder();
        }

        internal static RabbitQueueClientBuilder Create(Action<RabbitQueueClientBuilder> options)
        {
            RabbitQueueClientBuilder builder = new RabbitQueueClientBuilder();

            options.Invoke(builder);

            return builder;
        }

        public RabbitQueueClientBuilder UseConnection(Action<ConnectionFactory> options)
        {
            options.Invoke(_connectionFactory);

            return this;
        }

        public RabbitQueueClientBuilder AddEndpoints(Action<RabbitEndpointsBuilder> options)
        {
            options.Invoke(_endpointsBuilder);

            return this;
        }

        public RabbitQueueClientBuilder AddWorkers(Action<RabbitWorkersBuildler> options)
        {
            options.Invoke(_workersBuilder);

            return this;
        }

        internal IMessageQueueClient Build()
        {
            if (_connectionFactory is null)
                throw new ArgumentNullException(string.Empty, $"The connection factory can not be null. Configure the connection using the '{nameof(UseConnection)}' method.");

            return new RabbitQueueClient(_connectionFactory,
                                         _endpointsBuilder.Build(),
                                         _workersBuilder.Build());
        }
    }
}
