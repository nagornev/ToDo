namespace ToDo.MQ.RabbitMQ.Extensions
{
    internal static class DictionaryExtensions
    {
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, out TValue result)
        {
            result = default;

            if (dictionary.TryAdd(key, value))
            {
                result = value;
                return true;
            }

            return false;
        }
    }
}
