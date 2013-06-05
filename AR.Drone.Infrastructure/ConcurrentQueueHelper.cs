using System.Collections.Concurrent;

namespace AR.Drone.Infrastructure
{
    public static class ConcurrentQueueExtensions
    {
        public static void Flush<T>(this ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
            }
        }
    }
}