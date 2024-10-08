using System.Threading.Tasks;

namespace ToDo.MQ.Abstractions
{
    public interface IMessageQueueConsumeClient
    {
        Task Start();

        Task Stop();
    }
}
