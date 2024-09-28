using MQ.Abstractions;
using ToDo.MQ.Abstractions;

namespace ToDo.MQ.RabbitMQ
{
    public static class MesssageQueueBuilderExtensions
    {
        public static MessageQueueBuilder UseRabbit(this MessageQueueBuilder builder, Action<RabbitQueueClientBuilder> options)
        {
            RabbitQueueClientBuilder rabbit = RabbitQueueClientBuilder.Create(options);

            builder.UseMessageQueue(rabbit.Build());

            return builder;
        }
    }
}
