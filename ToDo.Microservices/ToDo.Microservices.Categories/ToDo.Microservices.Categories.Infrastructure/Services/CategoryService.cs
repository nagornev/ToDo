﻿using ToDo.Domain.Results;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Repositories;
using ToDo.Microservices.Categories.UseCases.Services;

namespace ToDo.Microservices.Categories.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<IEnumerable<Category>>> GetCategories(Guid userId)
        {
            return await _categoryRepository.Get(userId);
        }

        public async Task<Result<Category>> GetCategory(Guid userId, Guid categoryId)
        {
            return await _categoryRepository.Get(userId, categoryId);
        }

        public async Task<Result> CreateCategory(Guid userId, string name)
        {
            Result<Category> categoryResult = Category.New(name);

            return categoryResult.Success ?
                        await _categoryRepository.Create(userId, categoryResult.Content) :
                        categoryResult;
        }

        public async Task<Result> UpdateCategory(Guid userId, Guid categoryId, string name)
        {
            Result<Category> categoryResult = Category.Constructor(categoryId, name);

            return categoryResult.Success ?
                        await _categoryRepository.Update(userId, categoryResult.Content) :
                        categoryResult;

        }

        public async Task<Result> DeleteCategory(Guid userId, Guid categoryId)
        {
            return await _categoryRepository.Delete(userId, categoryId);
        }
    }
}
