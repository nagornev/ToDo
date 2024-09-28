using MQ.Abstractions;
using ToDo.MQ.RabbitMQ.Extensions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.MQ.Abstractions;

namespace ToDo.MQ.RabbitMQ
{
    public class RabbitWorkersBuildler
    {
        private RabbitEndpointsScheme _scheme;

        private List<RabbitWorkerProducer> _publishers;

        private List<RabbitWorkerProducer> _rpcs;

        private List<RabbitWorkerConsumer> _consumers;

        internal RabbitWorkersBuildler(Func<RabbitEndpointsScheme> scheme)
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
                                                          string routingKey = "",
                                                          Action<IBasicProperties> properties = null)
        {
            IRabbitExchange? exchange = _scheme.Exchanges.TryGetOne(exchangePredicate, out IRabbitExchange? result) ?
                                            result : throw new ArgumentException($"The exchange predicate returns more than one exchange.");

            _rpcs.Add(new RabbitWorkerProducer(typeof(TRequestType),
                                               exchange,
                                               routingKey,
                                               properties));

            return this;
        }

        public RabbitWorkersBuildler AddRPC<TRequestType>(Func<IRabbitQueue, bool> queuePredicate,
                                                          Action<IBasicProperties> properties = null)
        {
            IRabbitQueue queue = _scheme.Queues.TryGetOne(queuePredicate, out IRabbitQueue? result) ? 
                                            result! : throw new ArgumentException($"The queue predicate returns more than one exchange.");

            _rpcs.Add(new RabbitWorkerProducer(typeof(TRequestType),
                                               null,
                                               queue.Name,
                                               properties));

            return this;
        }

        public RabbitWorkersBuildler AddRPC<TRequestType>(string routingKey,
                                                          Action<IBasicProperties> properties = null)
        {
            _rpcs.Add(new RabbitWorkerProducer(typeof(TRequestType),
                                               null,
                                               routingKey,
                                               properties));

            return this;
        }

        public RabbitWorkersBuildler AddConsumer(Func<IRabbitQueue, bool> queuePredicate,
                                                 IMessageQueueConsumer consumer,
                                                 bool autoAck = true,
                                                 bool reply = false)
        {
            IRabbitQueue queue = _scheme.Queues.TryGetOne(queuePredicate, out IRabbitQueue? result) ?
                                            result! : throw new ArgumentException($"The queue predicate returns more than one exchange.");

            _consumers.Add(new RabbitWorkerConsumer(queue,
                                                    consumer,
                                                    autoAck,
                                                    reply));

            return this;
        }

        internal RabbitWorkerScheme Build()
        {
            return new RabbitWorkerScheme(_publishers, _rpcs, _consumers);
        }
    }
}
