using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ToDo.MQ.Abstractions
{
    public interface IMessageQueueClient : IDisposable
    {
        Task Publish<TMessageType>(TMessageType message);

        Task<IEnumerable<byte>> Send<TRequestType>(TRequestType request, CancellationToken cancellationToken = default);
    }
}
