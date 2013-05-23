using System.Collections.Concurrent;
using System.Threading;
using AI.Core.System;
using AR.Drone.Data;

namespace AR.Drone.Media
{
    public class PacketRecorder : WorkerBase
    {
        private readonly ConcurrentQueue<object> _packetQueue;
        private readonly string _path;

        public PacketRecorder(string path)
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
            _packetQueue.Flush();

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