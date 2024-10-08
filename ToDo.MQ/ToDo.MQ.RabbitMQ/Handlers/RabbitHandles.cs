namespace ToDo.MQ.RabbitMQ.Handlers
{
    internal class RabbitHandles : IRabbitHandles
    {
        public RabbitHandles(IReadOnlyCollection<IRabbitPublishHandler> publishers,
                             IReadOnlyCollection<IRabbitProcedureHandler> procedures,
                             IReadOnlyCollection<IRabbitConsumeHandler> consumers)
        {
            Publishers = publishers;
            Procedures = procedures;
            Consumers = consumers;
        }

        public IReadOnlyCollection<IRabbitPublishHandler> Publishers { get; private set; }

        public IReadOnlyCollection<IRabbitProcedureHandler> Procedures { get; private set; }

        public IReadOnlyCollection<IRabbitConsumeHandler> Consumers { get; private set; }

    }
}
