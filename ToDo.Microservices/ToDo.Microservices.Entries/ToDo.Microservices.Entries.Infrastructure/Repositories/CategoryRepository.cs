using Microsoft.Extensions.Options;
using Nagornev.Querer.Http;
using ToDo.Extensions;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.Infrastructure.Options;
using ToDo.Microservices.Entries.Querers.Queries.Categories;
using ToDo.Microservices.Entries.UseCases.Repositories;

namespace ToDo.Microservices.Entries.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private QuererHttpClient _client;

        private CategoryRepositoryOptions _options;

        public CategoryRepository(IQuererHttpClientFactory clientFactory, IOptions<CategoryRepositoryOptions> options)
        {
            _client = clientFactory.Create();
            _options = options.Value;
        }

        public async Task<IEnumerable<Category>> Get()
        {
            GetCategoriesCompiler compiler = new GetCategoriesCompiler($"http://{_options.Host}/Categories");
            GetCategoriesHandler handler = new GetCategoriesHandler();

            await _client.SendAsync(compiler, handler);

            return handler.Content;
        }

        public async Task<Category> Get(Guid categoryId)
        {
            GetCategoryCompiler compiler = new GetCategoryCompiler($"http://{_options.Host}/Categories", categoryId);
            GetCategoryHandler handler = new GetCategoryHandler();

            await _client.SendAsync(compiler, handler);

            return handler.Content;
        }
    }
}
