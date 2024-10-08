namespace ToDo.MQ.RabbitMQ.Handlers
{
    public interface IRabbitHandles
    {
        IReadOnlyCollection<IRabbitConsumeHandler> Consumers { get; }

        IReadOnlyCollection<IRabbitPublishHandler> Publishers { get; }

        IReadOnlyCollection<IRabbitProcedureHandler> Procedures { get; }
    }
}
