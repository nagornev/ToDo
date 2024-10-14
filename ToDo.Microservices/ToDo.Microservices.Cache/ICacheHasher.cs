namespace ToDo.Microservices.Cache
{
    public interface ICacheHasher<TKeyType>
    {
        string Hash(TKeyType key);
    }
}
