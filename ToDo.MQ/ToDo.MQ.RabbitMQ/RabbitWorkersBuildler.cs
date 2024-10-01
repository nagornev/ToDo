using RabbitMQ.Client;
using ToDo.MQ.Abstractions;
using ToDo.MQ.RabbitMQ.Extensions;

namespace ToDo.MQ.RabbitMQ
{
    public class RabbitWorkersBuildler
    {
        private RabbitEndpoints _scheme;

        private List<RabbitWorkerProducer> _publishers;

        private List<RabbitWorkerProducer> _rpcs;

        private List<RabbitWorkerConsumer> _consumers;

        internal RabbitWorkersBuildler(Func<RabbitEndpoints> scheme)
        {
            _scheme = scheme.Invoke();
            _publishers = new List<RabbitWorkerProducer>();
            _rpcs = new List<RabbitWorkerProducer>();
            _consumers = new List<RabbitWorkerConsumer>();
        }


        public RabbitWorkersBuildler AddPublisher<TMessageType>(Func<IRabbitExchange, bool> exchangePredicate,
                                                                string routingKey = "",
                                                                Action<IBasicProperties> properties = null)
        {
            IRabbitExchange exchange = _scheme.Exchanges.TryGetOne(exchangePredicate, out IRabbitExchange? result) ?
                                            result! : throw new ArgumentException($"The exchange predicate returns more than one exchange.");

            _publishers.Add(new RabbitWorkerProducer(typeof(TMessageType),
                                                     exchange,
                                                     routingKey,
                                                     properties));

            return this;
        }

        public RabbitWorkersBuildler AddPublisher<TMessageType>(string routingKey,
                                                                Action<IBasicProperties> properties = null)
        {
            _publishers.Add(new RabbitWorkerProducer(typeof(TMessageType),
                                                     null,
                                                     routingKey,
                                                     properties));

            return this;
        }

        public RabbitWorkersBuildler AddRPC<TRequestType>(Func<IRabbitExchange, bool> exchangePredicate,
                                                          Func<IRabbitQueue, bool> replyPredicate,
                                                          string routingKey = "")
        {
            IRabbitExchange? exchange = _scheme.Exchanges.TryGetOne(exchangePredicate, out IRabbitExchange? toExchange) ?
                                            toExchange : throw new ArgumentException($"The exchange predicate returns more than one exchange.");

            IRabbitQueue reply = _scheme.Queues.TryGetOne(replyPredicate, out IRabbitQueue replyQueue) ?
                                            replyQueue! : throw new ArgumentException($"The queue predicate returns more than one exchange.");



            _rpcs.Add(new RabbitWorkerProducer(typeof(TRequestType),
                                               exchange,
                                               routingKey,
                                               (props) =>
                                               {
                                                   props.CorrelationId = Guid.NewGuid().ToString();
                                                   props.ReplyTo = reply.Name;
                                               }));

            return this;
        }

        public RabbitWorkersBuildler AddRPC<TRequestType>(Func<IRabbitQueue, bool> queuePredicate,
                                                          Func<IRabbitQueue, bool> replyPredicate)
        {
            IRabbitQueue queue = _scheme.Queues.TryGetOne(queuePredicate, out IRabbitQueue? toQueue) ?
                                            toQueue! : throw new ArgumentException($"The queue predicate returns more than one exchange.");

            IRabbitQueue reply = _scheme.Queues.TryGetOne(replyPredicate, out IRabbitQueue replyQueue) ?
                                            replyQueue! : throw new ArgumentException($"The queue predicate returns more than one exchange.");



            _rpcs.Add(new RabbitWorkerProducer(typeof(TRequestType),
                                               null,
                                               queue.Name,
                                               (props) =>
                                               {
                                                   props.CorrelationId = Guid.NewGuid().ToString();
                                                   props.ReplyTo = reply.Name;
                                               }));

            return this;
        }

        public RabbitWorkersBuildler AddConsumer<TConsumerType>(Func<IRabbitQueue, bool> queuePredicate,
                                                                bool autoAck = true,
                                                                bool reply = false)
            where TConsumerType : IMessageQueueConsumer
        {
            IRabbitQueue queue = _scheme.Queues.TryGetOne(queuePredicate, out IRabbitQueue? result) ?
                                            result! : throw new ArgumentException($"The queue predicate returns more than one exchange.");

            _consumers.Add(new RabbitWorkerConsumer(queue,
                                                    typeof(TConsumerType),
                                                    autoAck,
                                                    reply));

            return this;
        }

        internal RabbitWorkers Build()
        {
            return new RabbitWorkers(_publishers, _rpcs, _consumers);
        }
    }
}
