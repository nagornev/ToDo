﻿using ToDo.Domain.Results;
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

        public async Task<Result<IEnumerable<EntryCompose>>> GetEntries()
        {
            IEnumerable<Entry> entries = await _entryRepository.Get();

            Result<IEnumerable<Category>> resultCategories = await _categoryService.Get();

            return resultCategories.Success ?
                    Result<IEnumerable<EntryCompose>>.Successful(_composer.Compose(entries, resultCategories.Content)) :
                    Result<IEnumerable<EntryCompose>>.Failure(resultCategories.Error);

        }

        public async Task<Result<EntryCompose>> GetEntry(Guid entryId)
        {
            Entry entry = await _entryRepository.Get(entryId);

            if (entry is null)
                return Result<EntryCompose>.Failure(Errors.IsNull($"The entry ({entryId}) was not found."));

            Result<Category> resultCategory = await _categoryService.Get(entry.CategoryId);

            return resultCategory.Success ?
                     Result<EntryCompose>.Successful(_composer.Compose(entry, resultCategory.Content)) :
                     Result<EntryCompose>.Failure(resultCategory.Error);
        }


        public async Task<Result> CreateEntry(Guid categoryId, string text, DateTime? deadline)
        { 
            Result<Category> resultCategory = await _categoryService.Get(categoryId);

            if (!resultCategory.Success)
                return resultCategory;

            return await _entryRepository.Create(Entry.New(categoryId, text, deadline)) ?
                    Result.Successful() :
                    Result.Failure(Errors.IsMessage("The entry was not created. Please check the entry parameters and try again later."));
        }


        public async Task<Result> UpdateEntry(Guid entryId, Guid categoryId, string text, DateTime? deadline, bool completed)
        {
            Result<Category> resultCategory = await _categoryService.Get(categoryId);

            if (!resultCategory.Success)
                return resultCategory;

            return await _entryRepository.Update(Entry.Constructor(entryId, categoryId, text, deadline, completed)) ?
                     Result.Successful() :
                     Result.Failure(Errors.IsMessage($"The entry ({entryId}) was not found."));
        }

        public async Task<Result> DeleteEntry(Guid entryId)
        {
            return await _entryRepository.Delete(entryId) ?
                    Result.Successful() :
                    Result.Failure(Errors.IsMessage($"The entry ({entryId}) was not found."));
        }
    }
}
