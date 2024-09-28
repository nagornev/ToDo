using System.Threading.Tasks;

namespace ToDo.MQ.Abstractions
{
    public interface IMessageQueueConsumer
    {
        Task Consume(IMessageQueueConsumerContext context);
    }
}
