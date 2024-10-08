using ToDo.Domain.Results;
using ToDo.Microservices.Entries.UseCases.Services;
using ToDo.Microservices.MQ;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Entries.Infrastructure.Consumers
{
    public class DeleteCategoryConsumer : MessageQueueStableConsumer
    {
        public const string Queue = "delete_category_entries_queue";

        private IEntryService _entryService;

        public DeleteCategoryConsumer(IEntryService entryRepository)
        {
            _entryService = entryRepository;
        }

        public async override Task Execute(IMessageQueueConsumerContext context)
        {
            DeleteCategoryPublishMessage message = context.GetMessage<DeleteCategoryPublishMessage>();

            Result deleteResult = await _entryService.DeleteEntriesByCategory(message.UserId, message.CategoryId);

            Complete(deleteResult.Success || deleteResult.Error.Key == Errors.IsNullKey,
                     context);
        }
    }
}

