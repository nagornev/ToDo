using ToDo.Domain.Results;
using ToDo.Microservices.Entries.UseCases.Services;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Entries.Infrastructure.Consumers
{
    public class NewUserConsumer : IMessageQueueConsumer
    {
        public const string Queue = "new_users_entries_queue";

        private IUserService _userService;

        public NewUserConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(IMessageQueueConsumerContext context)
        {
            NewUserPublish message = context.GetMessage<NewUserPublish>();

            Result result;

            do
            {
                result = await _userService.CreateUser(message.UserId);
            }
            while (!result.Success);

            context.Ack();
        }
    }
}
