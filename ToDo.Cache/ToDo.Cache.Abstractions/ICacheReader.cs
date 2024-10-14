using System.Threading.Tasks;
using ToDo.Domain.Results;

namespace ToDo.Cache.Abstractions
{
    public interface ICacheReader<TCacheType, THashType> : ICache<THashType>
    {
        Task<Result<TCacheType>> Get(THashType key);
    }
}
