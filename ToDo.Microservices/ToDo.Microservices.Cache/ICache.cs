namespace ToDo.Microservices.Cache
{
    public interface ICache<THashType>
    {
        ICacheHasher<THashType> Hasher { get; }
    }
}
