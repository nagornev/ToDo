using Microsoft.Extensions.Options;
using Nagornev.Querer.Http;
using ToDo.Domain.Results;
using ToDo.Extensions;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.Infrastructure.Options;
using ToDo.Microservices.Entries.Querers.Queries.Categories;
using ToDo.Microservices.Entries.UseCases.Services;

namespace ToDo.Microservices.Entries.Infrastructure.Services
{
    public class RestCategoryService : ICategoryService
    {
        private QuererHttpClient _client;

        private CategoryServiceOptions _options;

        public RestCategoryService(IQuererHttpClientFactory clientFactory,
                               IOptions<CategoryServiceOptions> options)
        {
            _client = clientFactory.Create();
            _options = options.Value;
        }

        public async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            GetCategoriesCompiler compiler = new GetCategoriesCompiler($"http://{_options.Host}/Categories");
            GetCategoriesHandler handler = new GetCategoriesHandler();

            await _client.SendAsync(compiler, handler);

            return handler.Content;
        }

        public async Task<Result<Category>> Get(Guid userId, Guid categoryId)
        {
            GetCategoryCompiler compiler = new GetCategoryCompiler($"http://{_options.Host}/Categories", categoryId);
            GetCategoryHandler handler = new GetCategoryHandler();

            await _client.SendAsync(compiler, handler);

            return handler.Content;
        }
    }
}
