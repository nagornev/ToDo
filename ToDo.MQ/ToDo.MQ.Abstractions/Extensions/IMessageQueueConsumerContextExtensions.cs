using System.Text;
using System.Text.Json;

namespace ToDo.MQ.Abstractions.Extensions
{
    public static class IMessageQueueConsumerContextExtensions
    {
        public static TMessageType GetMessage<TMessageType>(this IMessageQueueConsumerContext context)
        {
            return JsonSerializer.Deserialize<TMessageType>(context.Body);
        }

        public static void Respond<TMessageType>(this IMessageQueueConsumerContext context, TMessageType response)
        {
            context.Respond(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)));
        }
    }
}
