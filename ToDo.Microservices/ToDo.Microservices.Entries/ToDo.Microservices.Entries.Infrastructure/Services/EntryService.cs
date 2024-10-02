using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Collectings;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Providers;
using ToDo.Microservices.Entries.UseCases.Repositories;
using ToDo.Microservices.Entries.UseCases.Services;

namespace ToDo.Microservices.Entries.Infrastructure.Services
{
    public class EntryService : IEntryService
    {
        private IEntryRepository _entryRepository;

        private ICategoryService _categoryService;

        private IEntryСomposer _composer;

        public EntryService(IEntryRepository entryRepository,
                            ICategoryService categoryService,
                            IEntryСomposer сomposer)
        {
            _entryRepository = entryRepository;
            _categoryService = categoryService;
            _composer = сomposer;
        }

        public async Task<Result<IEnumerable<EntryCompose>>> GetEntries(Guid userId)
        {
            Result<IEnumerable<Entry>> entriesResult = await _entryRepository.Get(userId);
            Result<IEnumerable<Category>> categoriesResult = await _categoryService.Get(userId);

            return entriesResult.Success && categoriesResult.Success ?
                    Result<IEnumerable<EntryCompose>>.Successful(_composer.Compose(entriesResult.Content, categoriesResult.Content)) :
                    Result<IEnumerable<EntryCompose>>.Failure(entriesResult.Error ?? categoriesResult.Error);

        }

        public async Task<Result<EntryCompose>> GetEntry(Guid userId, Guid entryId)
        {
            Result<Entry> entryResult = await _entryRepository.Get(userId, entryId);

            if(!entryResult.Success)
                return Result<EntryCompose>.Failure(entryResult.Error);

            Result<Category> resultCategory = await _categoryService.Get(userId, entryResult.Content.CategoryId);

            return resultCategory.Success ?
                     Result<EntryCompose>.Successful(_composer.Compose(entryResult.Content, resultCategory.Content)) :
                     Result<EntryCompose>.Failure(resultCategory.Error);
        }


        public async Task<Result> CreateEntry(Guid userId, Guid categoryId, string text, DateTime? deadline)
        {
            Result<Category> categoryResult = await _categoryService.Get(userId, categoryId);

            return categoryResult.Success ?
                     await _entryRepository.Create(userId, Entry.New(categoryId, text, deadline)) :
                     categoryResult;
        }


        public async Task<Result> UpdateEntry(Guid userId, Guid entryId, Guid categoryId, string text, DateTime? deadline, bool completed)
        {
            Result<Category> categoryResult = await _categoryService.Get(userId, categoryId);

            return categoryResult.Success ?
                        await _entryRepository.Update(userId, Entry.Constructor(entryId, categoryId, text, deadline, completed)) :
                        categoryResult;
        }

        public async Task<Result> DeleteEntry(Guid userId, Guid entryId)
        {
            return await _entryRepository.Delete(userId, entryId);
        }
    }
}
