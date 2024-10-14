namespace ToDo.Microservices.Cache.Hashers
{
    public class CategoryCacheHasher : ICacheHasher<Guid>
    {
        private const string _tag = "Categories";

        public string Hash(Guid key)
        {
            return $"{_tag}-{key}";
        }
    }
}
