using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Repositories;
using ToDo.Microservices.MQ.Publishers;
using ToDo.MQ.Abstractions;
using ToDo.MQ.Abstractions.Extensions;

namespace ToDo.Microservices.Entries.Infrastructure.Consumers
{
    public class DeleteCategoryConsumer : IMessageQueueConsumer
    {
        public const string Queue = "delete_category_entries_queue";

        private IEntryRepository _entryRepository;

        public DeleteCategoryConsumer(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task Consume(IMessageQueueConsumerContext context)
        {
            DeleteCategoryPublish message = context.GetMessage<DeleteCategoryPublish>();

            Result<IEnumerable<Entry>> entriesResult;

            do
            {
                entriesResult = await _entryRepository.Get(message.UserId);
            }
            while (!entriesResult.Success);

            foreach (Entry entry in entriesResult.Content)
            {
                if (entry.CategoryId == message.CategoryId)
                    await _entryRepository.Delete(message.UserId, entry.Id);
            }

            context.Ack();
        }
    }
}
