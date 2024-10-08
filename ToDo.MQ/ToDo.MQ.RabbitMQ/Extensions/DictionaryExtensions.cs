using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.MQ.RabbitMQ.Extensions
{
    internal static class DictionaryExtensions
    {
        public static bool TryAdd<TKey,TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, out TValue result)
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
