using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ToDo.Cache.Abstractions;
using ToDo.Domain.Results;
using ToDo.Microservices.Cache.Hashers;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Infrastructure.Cachers
{
    public class CategoryCacheReader : ICacheReader<IEnumerable<Category>, Guid>
    {
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
                                      JsonSerializer.Deserialize<Result<IEnumerable<Category>>>(cache)! :
                                      Result<IEnumerable<Category>>.Failure(Errors.IsNull("No categories in cache."));
            }
            catch (Exception exception)
            {
                return HandleException(exception, (error) => Result<IEnumerable<Category>>.Failure(error));
            }
        }

        private string CreateHash(Guid key)
        {
            return Hasher.Hash(key);
        }

        private TResultType HandleException<TResultType>(Exception exception, Func<IError, TResultType> result)
            where TResultType : Result
        {
            _logger.LogError(exception, "The distributed cache service is unavaliable.");

            return result.Invoke(Errors.IsInternalServer("The distributed cache service is unavaliable."));
        }
    }
}
