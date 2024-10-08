using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace ToDo.MQ.Abstractions
{
    public interface IMessageQueueProcedureClient
    {
        Task<IEnumerable<byte>> Send<TRequestType>(TRequestType request, CancellationToken cancellationToken = default);
    }
}
