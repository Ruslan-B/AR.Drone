using System.Collections.Concurrent;

namespace AR.Drone.Client.Helpers
{
    public class ConcurrentQueueHelper
    {
        public static void Flush<T>(ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
            }
        }
    }
}