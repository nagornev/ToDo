using System.Threading.Tasks;

namespace ToDo.MQ.Abstractions
{
    public interface IMessageQueuePublishClient
    {
        Task Publish<TMessageType>(TMessageType message);
    }
}
