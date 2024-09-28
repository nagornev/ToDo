using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.MQ.RabbitMQ
{
    public interface IRabbitExchange
    {
        string Name { get; }

        string Type { get; }

        bool Durable { get; }

        bool AutoDelete { get; }

        IDictionary<string, object> Arguments { get; }

        IReadOnlyCollection<RabbitExchangeBind> Binds { get; }
    }
}
