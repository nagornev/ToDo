using ToDo.Domain.Results;

namespace ToDo.Microservices.Cache
{
    public interface ICacheReader<TCacheType, THashType> : ICache<THashType>
    {
        Task<Result<TCacheType>> Get(THashType key);
    }
}
