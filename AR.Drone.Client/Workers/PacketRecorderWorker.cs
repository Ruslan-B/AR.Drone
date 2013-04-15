using System.Collections.Concurrent;
using System.Threading;
using AI.Core.System;
using AR.Drone.Client.Helpers;
using AR.Drone.Client.IO;
using AR.Drone.Client.Packets;

namespace AR.Drone.Client.Workers
{
    public class PacketRecorderWorker : WorkerBase
    {
        private readonly ConcurrentQueue<object> _packetQueue;
        private readonly string _path;

        public PacketRecorderWorker(string path)
        {
            _path = path;
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

            using (var recorder = new PacketWriter(_path))
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