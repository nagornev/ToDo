namespace ToDo.MQ.RabbitMQ
{
    public class RabbitQueue : IRabbitQueue
    {
        private List<RabbitQueueBind> _binds;

        public RabbitQueue(string name,
                           bool durable,
                           bool exclusive,
                           bool autoDelete,
                           IDictionary<string, object> arguments)
        {
            Name = name;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Arguments = arguments;

            _binds = new List<RabbitQueueBind>();
        }

        public string Name { get; private set; }

        public bool Durable { get; private set; }

        public bool Exclusive { get; private set; }

        public bool AutoDelete { get; private set; }

        public IDictionary<string, object> Arguments { get; private set; }

        public IReadOnlyCollection<RabbitQueueBind> Binds => _binds;

        internal void AddBind(RabbitQueueBind bind)
        {
            _binds.Add(bind);
        }
    }
}
