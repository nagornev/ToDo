namespace ToDo.MQ.RabbitMQ.Endpoints
{
    public class RabbitQueue : IRabbitQueue
    {
        private List<IRabbitQueueBind> _binds;

        public RabbitQueue(string name,
                           bool durable,
                           bool exclusive,
                           bool autoDelete,
                           Dictionary<string, object> arguments)
        {
            Name = name;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Arguments = arguments;

            _binds = new List<IRabbitQueueBind>();
        }

        public string Name { get; private set; }

        public bool Durable { get; private set; }

        public bool Exclusive { get; private set; }

        public bool AutoDelete { get; private set; }

        public IDictionary<string, object> Arguments { get; private set; }

        public IReadOnlyCollection<IRabbitQueueBind> Binds => _binds;

        internal void AddBind(IRabbitQueueBind bind)
        {
            _binds.Add(bind);
        }
    }
}
