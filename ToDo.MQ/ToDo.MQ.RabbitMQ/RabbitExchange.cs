namespace ToDo.MQ.RabbitMQ
{
    public class RabbitExchange: IRabbitExchange
    {
        private List<RabbitExchangeBind> _binds;

        public RabbitExchange(string name,
                              string type,
                              bool durable,
                              bool autoDelete,
                              IDictionary<string, object> arguments)
        {
            Name = name;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;

            _binds = new List<RabbitExchangeBind>();
        }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public bool Durable { get; private set; }

        public bool AutoDelete { get; private set; }

        public IDictionary<string, object> Arguments { get; private set; }

        public IReadOnlyCollection<RabbitExchangeBind> Binds => _binds;

        internal void AddBind(RabbitExchangeBind bind)
        {
            _binds.Add(bind);
        }
    }
}
