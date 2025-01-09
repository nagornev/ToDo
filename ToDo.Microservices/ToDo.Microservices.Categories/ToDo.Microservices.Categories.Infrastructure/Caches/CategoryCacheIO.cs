using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ToDo.Cache.Abstractions;
using ToDo.Domain.Results;
using ToDo.Microservices.Cache.Hashers;
using ToDo.Microservices.Categories.Domain.Models;
using ToDo.Microservices.Categories.UseCases.Caches;

namespace ToDo.Microservices.Categories.Infrastructure.Caches
{
    public class CategoryCacheIO : ICategoryCacheIO
    {
        private const string _internalServerMessage = "The distributed cache service is unavaliable.";

        private const int _cacheLifetime = 600000;

        private IDistributedCache _cache;

        private ILogger<CategoryCacheIO> _logger;

        private DistributedCacheEntryOptions _options;

        public CategoryCacheIO(CategoryCacheHasher hasher,
                               IDistributedCache cache,
                               ILogger<CategoryCacheIO> logger)
        {
            Hasher = hasher;
            _cache = cache;
            _logger = logger;

            _options = (new DistributedCacheEntryOptions()).SetSlidingExpiration(TimeSpan.FromMilliseconds(_cacheLifetime));
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

        public async Task<Result> Set(Guid userId, Result<IEnumerable<Category>> categoriesResult)
        {
            try
            {
                string cache = categoriesResult.ToString();

                await _cache.SetStringAsync(CreateHash(userId), cache, _options);

                return Result.Successful();
            }
            catch (Exception exception)
            {
                return HandleException(exception, () => Result.Failure(error => error.InternalServer(_internalServerMessage)));
            }
        }

        public async Task<Result> Remove(Guid userId)
        {
            try
            {
                await _cache.RemoveAsync(CreateHash(userId));

                return Result.Successful();
            }
            catch (Exception exception)
            {
                return HandleException(exception, () => Result.Failure(error => error.InternalServer(_internalServerMessage)));
            }
        }

        private string CreateHash(Guid key)
        {
            return Hasher.Hash(key);
        }

        private TResultType HandleException<TResultType>(Exception exception, Func<TResultType> resultFactory)
          where TResultType : Result
        {
            _logger.LogError(exception, _internalServerMessage);

            return resultFactory.Invoke();
        }
    }
}
