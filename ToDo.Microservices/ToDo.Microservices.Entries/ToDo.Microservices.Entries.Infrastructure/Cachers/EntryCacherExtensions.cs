using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Cachers;

namespace ToDo.Microservices.Entries.Infrastructure.Cachers
{
    internal static class EntryCacherExtensions
    {
        public static async Task<Result<Entry>> Get(this IEntryCacher cacher, Guid userId, Guid entryId)
        {
            Result<IEnumerable<Entry>> entriesResult = await cacher.Get(userId);

            if (entriesResult.Success)
            {
                Entry? searched = entriesResult.Content.FirstOrDefault(x => x.Id == entryId);

                return searched is not null ?
                          Result<Entry>.Successful(searched) :
                          Result<Entry>.Failure(Errors.IsNull($"The entry ({entryId}) was not found."));
            }

            return Result<Entry>.Failure(entriesResult.Error);
        }
    }
}
