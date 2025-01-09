using System.Threading.Tasks;
using ToDo.Domain.Results;

namespace ToDo.Cache.Abstractions
{
    public interface ICacheWriter<TCacheType, THashType> : ICache<THashType>
    {
        Task<Result> Set(THashType key, Result<TCacheType> cacheResult);

        Task<Result> Remove(THashType key);
    }
}
