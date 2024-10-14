namespace ToDo.Cache.Abstractions
{
    public interface ICacheHasher<TKeyType>
    {
        string Hash(TKeyType key);
    }
}
