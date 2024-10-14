namespace ToDo.Microservices.Cache.Hashers
{
    public class EntryCacheHasher : ICacheHasher<Guid>
    {
        private const string _tag = "Entries";

        public string Hash(Guid key)
        {
            return $"{_tag}-{key}";
        }
    }
}
