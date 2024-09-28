using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.MQ.RabbitMQ.Extensions
{
    public static class CollectionsExtension
    {
        public static bool TryGetValue<T>(this IEnumerable<T> collection, Func<T, bool> predicate, out T? result)
            where T : class
        {
            result = null;

            foreach (T value in collection)
            {
                if (predicate.Invoke(value))
                {
                    result = value;
                    break;
                }
            }

            return !(result is null) ?
                      true :
                      false;
                    
        }

        public static bool TryGetOne<T>(this IEnumerable<T> collection, Func<T, bool> predicate, out T? result)
            where T : class
        {
            var select = collection.Where(predicate.Invoke);

            result = select.Count() == 1?
                        select.First():
                        null;

            return result != null;
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
           where T : class
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}
