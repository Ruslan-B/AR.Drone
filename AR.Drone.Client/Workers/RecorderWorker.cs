using System;
using System.Collections.Concurrent;
using System.Threading;
using AI.Core.System;
using AR.Drone.Client.Helpers;
using AR.Drone.Client.IO;
using AR.Drone.Client.Navigation;
using AR.Drone.Client.Video;

namespace AR.Drone.Client.Workers
{
    public class RecorderWorker : WorkerBase
    {
        private readonly ConcurrentQueue<object> _packetQueue;

        public RecorderWorker()
        {
            _packetQueue = new ConcurrentQueue<object>();
        }

        public void EnqueuePacket(NavigationPacket packet)
        {
            _packetQueue.Enqueue(packet);
        }

        public void EnqueuePacket(VideoPacket packet)
        {
            _packetQueue.Enqueue(packet);
        }

        protected override void Loop(CancellationToken token)
        {
            ConcurrentQueueHelper.Flush(_packetQueue);

            string path = string.Format("ardrone_{0:yyyy-MM-dd-HH-mm-ss}.track", DateTime.Now);
            using (var recorder = new PacketWriter(path))
            {
                while (token.IsCancellationRequested == false)
                {
                    object packet;
                    while (_packetQueue.TryDequeue(out packet))
                    {
                        recorder.Write(packet);
                    }
                    Thread.Sleep(1);
                }
            }
        }
    }
}