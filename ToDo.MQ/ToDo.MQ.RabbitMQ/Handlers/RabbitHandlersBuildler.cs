using RabbitMQ.Client;
using ToDo.MQ.Abstractions;
using ToDo.MQ.RabbitMQ.Endpoints;
using ToDo.MQ.RabbitMQ.Extensions;

namespace ToDo.MQ.RabbitMQ.Handlers
{
    public class RabbitHandlersBuildler
    {
        private IRabbitEndpoints _endpoints;

        private List<IRabbitPublishHandler> _publishers;

        private List<IRabbitProcedureHandler> _procedures;

        private List<IRabbitConsumeHandler> _consumers;

        internal RabbitHandlersBuildler(Func<IRabbitEndpoints> scheme)
        {
            _endpoints = scheme.Invoke();
            _publishers = new List<IRabbitPublishHandler>();
            _procedures = new List<IRabbitProcedureHandler>();
            _consumers = new List<IRabbitConsumeHandler>();
        }


        public RabbitHandlersBuildler AddPublisher<TMessageType>(Func<IRabbitExchange, bool> exchangePredicate,
                                                                 string routingKey = "",
                                                                 Action<IBasicProperties> properties = null)
        {
            IRabbitExchange exchange = _endpoints.Exchanges.TryGetOne(exchangePredicate, out IRabbitExchange? result) ?
                                            result! : throw new ArgumentException($"The exchange predicate returns more than one exchange.");

            _publishers.Add(new RabbitPublishHandler(typeof(TMessageType),
                                                     exchange,
                                                     routingKey,
                                                     properties));

            return this;
        }

        public RabbitHandlersBuildler AddPublisher<TMessageType>(string routingKey,
                                                                Action<IBasicProperties> properties = null)
        {
            _publishers.Add(new RabbitPublishHandler(typeof(TMessageType),
                                                     null,
                                                     routingKey,
                                                     properties));

            return this;
        }

        public RabbitHandlersBuildler AddProcedure<TRequestType>(Func<IRabbitExchange, bool> exchangePredicate,
                                                                 Func<IRabbitQueue, bool> replyPredicate,
                                                                 string routingKey = "")
        {
            IRabbitExchange? exchange = _endpoints.Exchanges.TryGetOne(exchangePredicate, out IRabbitExchange? toExchange) ?
                                            toExchange : throw new ArgumentException($"The exchange predicate returns more than one exchange.");

            IRabbitQueue reply = _endpoints.Queues.TryGetOne(replyPredicate, out IRabbitQueue replyQueue) ?
                                            replyQueue! : throw new ArgumentException($"The queue predicate returns more than one exchange.");



            _procedures.Add(new RabbitProcedureHandler(typeof(TRequestType),
                                                 exchange,
                                                 routingKey,
                                                 (props) =>
                                                 {
                                                     props.CorrelationId = Guid.NewGuid().ToString();
                                                     props.ReplyTo = reply.Name;
                                                 }));

            return this;
        }

        public RabbitHandlersBuildler AddProcedure<TRequestType>(Func<IRabbitQueue, bool> queuePredicate,
                                                                 Func<IRabbitQueue, bool> replyPredicate)
        {
            IRabbitQueue queue = _endpoints.Queues.TryGetOne(queuePredicate, out IRabbitQueue? toQueue) ?
                                            toQueue! : throw new ArgumentException($"The queue predicate returns more than one exchange.");

            IRabbitQueue reply = _endpoints.Queues.TryGetOne(replyPredicate, out IRabbitQueue replyQueue) ?
                                            replyQueue! : throw new ArgumentException($"The queue predicate returns more than one exchange.");



            _procedures.Add(new RabbitProcedureHandler(typeof(TRequestType),
                                                 null,
                                                 queue.Name,
                                                 (props) =>
                                                 {
                                                     props.CorrelationId = Guid.NewGuid().ToString();
                                                     props.ReplyTo = reply.Name;
                                                 }));

            return this;
        }

        public RabbitHandlersBuildler AddConsumer<TConsumerType>(Func<IRabbitQueue, bool> queuePredicate,
                                                                bool autoAck = true,
                                                                bool reply = false)
            where TConsumerType : IMessageQueueConsumer
        {
            IRabbitQueue queue = _endpoints.Queues.TryGetOne(queuePredicate, out IRabbitQueue? result) ?
                                            result! : throw new ArgumentException($"The queue predicate returns more than one exchange.");

            _consumers.Add(new RabbitConsumeHandler(queue,
                                                    typeof(TConsumerType),
                                                    autoAck,
                                                    reply));

            return this;
        }

        internal IRabbitHandles Build()
        {
            return new RabbitHandles(_publishers, _procedures, _consumers);
        }
    }
}
