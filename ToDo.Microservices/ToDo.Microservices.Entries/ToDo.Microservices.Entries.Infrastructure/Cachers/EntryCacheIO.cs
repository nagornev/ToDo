﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ToDo.Domain.Results;
using ToDo.Microservices.Cache;
using ToDo.Microservices.Cache.Hashers;
using ToDo.Microservices.Entries.Domain.Models;

namespace ToDo.Microservices.Entries.Infrastructure.Cachers
{
    public class EntryCacheIO : ICacheIO<IEnumerable<Entry>, Guid>
    {
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
                          JsonSerializer.Deserialize<Result<IEnumerable<Entry>>>(cache)! :
                          Result<IEnumerable<Entry>>.Failure(Errors.IsNull("No entries in cache."));
            }
            catch(Exception exception)
            {
                return HandleException(exception, (error) => Result<IEnumerable<Entry>>.Failure(error));
            }
        }

        public async Task<Result> Set(Guid userId, Result<IEnumerable<Entry>> entriesResult)
        {
            try
            {
                string cache = JsonSerializer.Serialize(entriesResult);

                await _cache.SetStringAsync(CreateHash(userId), cache, _options);

                return Result.Successful();
            }
            catch (Exception exception)
            {
                return HandleException(exception, (error) => Result.Failure(error));
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
                return HandleException(exception, (error) => Result.Failure(error));
            }
        }

        private string CreateHash(Guid userId)
        {
            return Hasher.Hash(userId);
        }

        private TResultType HandleException<TResultType>(Exception exception, Func<IError, TResultType> result)
            where TResultType : Result
        {
            _logger.LogError(exception, "The distributed cache service is unavaliable.");

            return result.Invoke(Errors.IsInternalServer("The distributed cache service is unavaliable."));
        }
    }
}
