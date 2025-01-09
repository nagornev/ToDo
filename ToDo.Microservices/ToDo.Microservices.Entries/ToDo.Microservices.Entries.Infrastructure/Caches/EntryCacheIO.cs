using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ToDo.Cache.Abstractions;
using ToDo.Domain.Results;
using ToDo.Microservices.Cache.Hashers;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Caches;

namespace ToDo.Microservices.Entries.Infrastructure.Caches
{
    public class EntryCacheIO : IEntryCacheIO
    {
        private const string _internalServerMessage = "The distributed cache service is unavaliable.";

        private const int _cacheLifetime = 600000;

        private IDistributedCache _cache;

        private ILogger<EntryCacheIO> _logger;

        private DistributedCacheEntryOptions _options;

        public EntryCacheIO(EntryCacheHasher hasher,
                            IDistributedCache cache, 
                            ILogger<EntryCacheIO> logger)
        {
            Hasher = hasher;
            _cache = cache;
            _logger = logger;

            _options = (new DistributedCacheEntryOptions()).SetSlidingExpiration(TimeSpan.FromMilliseconds(_cacheLifetime));
        }

        public ICacheHasher<Guid> Hasher { get; private set; }

        public async Task<Result<IEnumerable<Entry>>> Get(Guid userId)
        {
            try
            {
                string? cache = await _cache.GetStringAsync(CreateHash(userId));

                return !string.IsNullOrEmpty(cache) ?
                          Result<IEnumerable<Entry>>.Deserialize(cache)! :
                          Result<IEnumerable<Entry>>.Failure(error => error.NullOrEmpty("No entries in cache."));
            }
            catch(Exception exception)
            {
                return HandleException(exception, () => Result<IEnumerable<Entry>>.Failure(error => error.InternalServer(_internalServerMessage)));
            }
        }

        public async Task<Result> Set(Guid userId, Result<IEnumerable<Entry>> entriesResult)
        {
            try
            {
                string cache = entriesResult.ToString();

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

        private string CreateHash(Guid userId)
        {
            return Hasher.Hash(userId);
        }

        private TResultType HandleException<TResultType>(Exception exception, Func<TResultType> result)
            where TResultType : Result
        {
            _logger.LogError(exception, _internalServerMessage);

            return result.Invoke();
        }
    }
}
