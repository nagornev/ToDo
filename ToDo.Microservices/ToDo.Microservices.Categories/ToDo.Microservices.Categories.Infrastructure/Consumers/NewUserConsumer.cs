using ToDo.Domain.Results;
using ToDo.Microservices.Categories.UseCases.Services;
using ToDo.Microservices.MQ;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Categories.Infrastructure.Consumers
{
    public class NewUserConsumer : MessageQueueStableConsumer
    {
        public const string Queue = "new_users_categories_queue";

        private IUserService _userService;

        public NewUserConsumer(IUserService userService)
        {
            _userService = userService;
        }

        public async override Task Execute(IMessageQueueConsumerContext context)
        {
            NewUserPublishMessage message = context.GetMessage<NewUserPublishMessage>();

            Result result = await _userService.CreateUser(message.User.Id);

            Complete(result.Success, context);
        }
    }
}
