using ToDo.MQ.Abstractions;

namespace ToDo.MQ.RabbitMQ
{
    public static class MesssageQueueBuilderExtensions
    {
        public static MessageQueueBuilder UseRabbit(this MessageQueueBuilder builder, Action<RabbitQueueClientBuilder> options)
        {
            RabbitQueueClientBuilder rabbit = RabbitQueueClientBuilder.Create(builder.GetServices());

            options.Invoke(rabbit);

            rabbit.Build();

            return builder;
        }
    }
}
