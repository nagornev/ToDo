using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ToDo.MQ.Abstractions
{
    public interface IMessageQueueProcedureClient
    {
        Task<IEnumerable<byte>> Send<TRequestType>(TRequestType request, CancellationToken cancellationToken = default);
    }
}
