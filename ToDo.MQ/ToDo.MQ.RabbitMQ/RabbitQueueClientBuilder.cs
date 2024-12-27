using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using ToDo.MQ.Abstractions;
using ToDo.MQ.RabbitMQ.Clients;
using ToDo.MQ.RabbitMQ.Endpoints;
using ToDo.MQ.RabbitMQ.Handlers;

namespace ToDo.MQ.RabbitMQ
{
    public class RabbitQueueClientBuilder
    {
        private IServiceCollection _services;

        private ConnectionFactory _connectionFactory;

        private RabbitEndpointsBuilder _endpointsBuilder;

        private RabbitHandlersBuildler _handlersBuilder;

        private RabbitQueueClientBuilder(IServiceCollection services)
        {
            _services = services;

            _connectionFactory = new ConnectionFactory();
            _endpointsBuilder = new RabbitEndpointsBuilder();
            _handlersBuilder = new RabbitHandlersBuildler(() => _endpointsBuilder.Build());
        }

        internal static RabbitQueueClientBuilder Create(IServiceCollection services)
        {
            return new RabbitQueueClientBuilder(services);
        }

        public RabbitQueueClientBuilder SetConnection(Action<ConnectionFactory> options)
        {
            options.Invoke(_connectionFactory);

            return this;
        }

        public RabbitQueueClientBuilder AddEndpoints(Action<RabbitEndpointsBuilder> options)
        {
            options.Invoke(_endpointsBuilder);

            return this;
        }

        public RabbitQueueClientBuilder AddHandlers(Action<RabbitHandlersBuildler> options)
        {
            options.Invoke(_handlersBuilder);

            return this;
        }

        internal void Build()
        {
            if (_connectionFactory is null)
                throw new ArgumentNullException(string.Empty, $"The connection factory can not be null. Configure the connection using the '{nameof(SetConnection)}' method.");


            IRabbitEndpoints endpoints = _endpointsBuilder.Build();
            IRabbitHandles handlers = _handlersBuilder.Build();

            _services.AddSingleton<IRabbitScheme>(provider => new RabbitScheme(_connectionFactory, endpoints));

            _services.AddSingleton<IReadOnlyCollection<IRabbitPublishHandler>>(handlers.Publishers);
            _services.AddSingleton<IReadOnlyCollection<IRabbitProcedureHandler>>(handlers.Procedures);
            _services.AddSingleton<IReadOnlyCollection<IRabbitConsumeHandler>>(handlers.Consumers);

            _services.AddSingleton<IMessageQueuePublishClient, RabbitQueuePublishClient>();
            _services.AddSingleton<IMessageQueueProcedureClient, RabbitQueueProcedureClient>();
            _services.AddSingleton<IMessageQueueConsumeClient, RabbitQueueConsumeClient>();
        }
    }
}
