namespace ToDo.MQ.RabbitMQ.Endpoints
{
    public class RabbitExchange : IRabbitExchange
    {
        private List<IRabbitExchangeBind> _binds;

        public RabbitExchange(string name,
                              string type,
                              bool durable,
                              bool autoDelete,
                              Dictionary<string, object> arguments)
        {
            Name = name;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;

            _binds = new List<IRabbitExchangeBind>();
        }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public bool Durable { get; private set; }

        public bool AutoDelete { get; private set; }

        public IDictionary<string, object> Arguments { get; private set; }

        public IReadOnlyCollection<IRabbitExchangeBind> Binds => _binds;

        internal void AddBind(IRabbitExchangeBind bind)
        {
            _binds.Add(bind);
        }
    }
}
