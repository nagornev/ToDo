using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Domain.Results;
using ToDo.Domain.Results.Extensions;

namespace ToDo.Cache.Abstractions.Extensions
{
    public static class CacheReaderExtensions
    {
        private const string _notFoundMessage = "The distributed cache has not record with this predicate.";

        public static async Task<Result<TCacheType>> Get<TCacheType, THashType>(this ICacheReader<IEnumerable<TCacheType>, THashType> cacheReader, THashType key, Func<TCacheType, bool> predicate, string notFoundMessage = _notFoundMessage)
        {
            Result<IEnumerable<TCacheType>> cacheResult = await cacheReader.Get(key);

            if (!cacheResult.Success)
                return Result<TCacheType>.Failure(cacheResult.Error);

            TCacheType searchedCache = cacheResult.Content.FirstOrDefault(x => predicate.Invoke(x));

            return searchedCache != null ?
                       Result<TCacheType>.Successful(searchedCache) :
                       Result<TCacheType>.Failure(error => error.NullOrEmpty(notFoundMessage));
        }
    }
}
