namespace ToDo.MQ.RabbitMQ
{
    internal class RabbitWorkers
    {
        public RabbitWorkers(IReadOnlyCollection<RabbitWorkerProducer> publishers,
                                  IReadOnlyCollection<RabbitWorkerProducer> rpcs,
                                  IReadOnlyCollection<RabbitWorkerConsumer> consumers)
        {
            Publishers = publishers;
            RPCs = rpcs;
            Consumers = consumers;
        }

        public IReadOnlyCollection<RabbitWorkerProducer> Publishers { get; private set; }

        public IReadOnlyCollection<RabbitWorkerProducer> RPCs { get; private set; }

        public IReadOnlyCollection<RabbitWorkerConsumer> Consumers { get; private set; }
    }
}
