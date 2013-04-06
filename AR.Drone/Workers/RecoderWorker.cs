using System;
using System.Collections.Concurrent;
using System.Threading;
using AR.Drone.Common;
using AR.Drone.Helpers;
using AR.Drone.IO;
using AR.Drone.Navigation;
using AR.Drone.Video;

namespace AR.Drone.Workers
{
    public class RecoderWorker : WorkerBase
    {
        private readonly ConcurrentQueue<object> _packetQueue;

        public RecoderWorker()
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

            string path = string.Format("ardrone_{0:yyyy-MM-dd-HH-mm-ss}.rec", DateTime.Now);
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