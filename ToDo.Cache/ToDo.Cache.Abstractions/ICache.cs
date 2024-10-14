namespace ToDo.Cache.Abstractions
{
    public interface ICache<THashType>
    {
        ICacheHasher<THashType> Hasher { get; }
    }
}
