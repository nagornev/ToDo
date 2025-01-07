using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ToDo.Cache.Abstractions;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Extensions;
using ToDo.Microservices.Cache.Hashers;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Caches;

namespace ToDo.Microservices.Entries.Infrastructure.Caches
{
    public class CategoryCacheReader : ICategoryCacheReader
    {
        private const string _internalServerMessage = "The distributed cache service is unavaliable.";

        private IDistributedCache _cache;

        private ILogger<CategoryCacheReader> _logger;

        public CategoryCacheReader(CategoryCacheHasher hasher,
                                   IDistributedCache cache,
                                   ILogger<CategoryCacheReader> logger)
        {
            Hasher = hasher;
            _cache = cache;
            _logger = logger;
        }

        public ICacheHasher<Guid> Hasher { get; private set; }

        public async Task<Result<IEnumerable<Category>>> Get(Guid userId)
        {
            try
            {
                string? cache = await _cache.GetStringAsync(CreateHash(userId));

                return !string.IsNullOrEmpty(cache) ?
                                      Result<IEnumerable<Category>>.Deserialize(cache)! :
                                      Result<IEnumerable<Category>>.Failure(error => error.NullOrEmpty("No categories in cache."));
            }
            catch (Exception exception)
            {
                return HandleException(exception, () => Result<IEnumerable<Category>>.Failure(error => error.InternalServer(_internalServerMessage)));
            }
        }

        private string CreateHash(Guid key)
        {
            return Hasher.Hash(key);
        }

        private TResultType HandleException<TResultType>(Exception exception, Func<TResultType> result)
            where TResultType : Result
        {
            _logger.LogError(exception, _internalServerMessage);

            return result.Invoke();
        }
    }
}
