using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ToDo.Domain.Results;
using ToDo.Microservices.Entries.Domain.Models;
using ToDo.Microservices.Entries.UseCases.Cachers;

namespace ToDo.Microservices.Entries.Infrastructure.Cachers
{
    public class EntryCacher : IEntryCacher
    {
        private const int _cacheLifetime = 600000;

        private IDistributedCache _cache;

        private DistributedCacheEntryOptions _options;

        private ILogger<EntryCacher> _logger;

        public EntryCacher(IDistributedCache cache, 
                           //IOptions<DistributedCacheEntryOptions> options,
                           ILogger<EntryCacher> logger)
        {
            _cache = cache;
            _options = /*options.Value ??*/ (new DistributedCacheEntryOptions()).SetSlidingExpiration(TimeSpan.FromMilliseconds(_cacheLifetime));
            _logger = logger;
        }

        public async Task<Result<IEnumerable<Entry>>> Get(Guid userId)
        {
            try
            {
                string? cache = await _cache.GetStringAsync(CreateKey(userId));

                return !string.IsNullOrEmpty(cache) ?
                          JsonSerializer.Deserialize<Result<IEnumerable<Entry>>>(cache)! :
                          Result<IEnumerable<Entry>>.Failure(Errors.IsNull("No entries cache."));
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

                await _cache.SetStringAsync(CreateKey(userId), cache, _options);

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
                await _cache.RemoveAsync(CreateKey(userId));

                return Result.Successful();
            }
            catch (Exception exception)
            {
                return HandleException(exception, (error) => Result.Failure(error));
            }
        }

        private string CreateKey(Guid userId)
        {
            return $"{nameof(EntryCacher)}-{userId}";
        }

        private TResultType HandleException<TResultType>(Exception exception, Func<IError, TResultType> result)
            where TResultType : Result
        {
            _logger.LogError(exception, "The distributed cache service is unavaliable.");

            return result.Invoke(Errors.IsInternalServer("The distributed cache service is unavaliable."));
        }
    }
}
