using System.Collections.Concurrent;

namespace AR.Drone.Helpers
{
    public class ConcurrentQueueHelper
    {
        public static void Clear<T>(ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
            }
        }
    }
}