using ToDo.Domain.Results;

namespace ToDo.Microservices.Cache.Extensions
{
    public static class CacheReaderExtensions
    {
        private const string _notFoundMessage = "The distributed cache has not record with this predicate.";

        public static async Task<Result<TCacheType>> Get<TCacheType, THashType>(this ICacheReader<IEnumerable<TCacheType>, THashType> cacheReader, THashType key, Func<TCacheType, bool> predicate, string notFoundMessage = _notFoundMessage)
        {
            Result<IEnumerable<TCacheType>> cacheResult = await cacheReader.Get(key);

            if (!cacheResult.Success)
                return Result<TCacheType>.Failure(cacheResult.Error);

            TCacheType? searchedCache = cacheResult.Content.FirstOrDefault(x => predicate.Invoke(x));

            return searchedCache is not null ?
                       Result<TCacheType>.Successful(searchedCache) :
                       Result<TCacheType>.Failure(Errors.IsNull(notFoundMessage));
        }
    }
}
