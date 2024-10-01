using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using ToDo.MQ.Abstractions;

namespace ToDo.MQ.RabbitMQ
{
    public class RabbitQueueClientBuilder
    {
        private IServiceCollection _services;

        private ConnectionFactory _connectionFactory;

        private RabbitEndpointsBuilder _endpointsBuilder;

        private RabbitWorkersBuildler _workersBuilder;

        private RabbitQueueClientBuilder(IServiceCollection services)
        {
            _services = services;

            _connectionFactory = new ConnectionFactory();
            _endpointsBuilder = new RabbitEndpointsBuilder();
            _workersBuilder = new RabbitWorkersBuildler(() => _endpointsBuilder.Build());
        }

        internal static RabbitQueueClientBuilder Create(IServiceCollection services)
        {
            return new RabbitQueueClientBuilder(services);
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

        internal void Build()
        {
            if (_connectionFactory is null)
                throw new ArgumentNullException(string.Empty, $"The connection factory can not be null. Configure the connection using the '{nameof(UseConnection)}' method.");

            var endpoints = _endpointsBuilder.Build();

            _services.AddSingleton(provider => new RabbitScheme(_connectionFactory, endpoints.Exchanges, endpoints.Queues));
            _services.AddSingleton(provider => _workersBuilder.Build());
            _services.AddSingleton<IMessageQueueClient, RabbitQueueClient>();
        }
    }
}
