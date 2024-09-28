using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ToDo.MQ.Abstractions;

namespace MQ.Abstractions.Extensions
{
    public static class IMessageQueueClientExtensions
    {
        public static async Task<TResponseType> Send<TResponseType, TRequestType>(this IMessageQueueClient messageQueue, TRequestType request, CancellationToken cancellationToken = default)
        {
            IEnumerable<byte> resposne = await messageQueue.Send(request, cancellationToken);

            return JsonSerializer.Deserialize<TResponseType>(resposne.ToArray());
        }
    }
}
