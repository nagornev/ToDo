using ToDo.Domain.Results;

namespace ToDo.Microservices.Cache
{
    public interface ICacheWriter<TCacheType, THashType>: ICache<THashType>
    {
        Task<Result> Set(THashType key, Result<TCacheType> cacheResult);

        Task<Result> Remove(THashType key);
    }
}
