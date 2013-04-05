using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using AR.Drone.Common;
using AR.Drone.Helpers;
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

            string filename = string.Format("ardrone_{0:yyyy-MM-dd-HH-mm}.rec", DateTime.Now);
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                var writer = new BinaryWriter(stream);
                while (token.IsCancellationRequested == false)
                {
                    object packet;
                    while (_packetQueue.TryDequeue(out packet))
                    {
                        if (packet is NavigationPacket)
                        {
                            writer.Write((byte) RecorderPacketType.Navigation);
                            var np = (NavigationPacket) packet;
                            writer.Write(np.Timestamp);
                            writer.Write(np.Data.Length);
                            writer.Write(np.Data);
                        }
                        else if (packet is VideoPacket)
                        {
                            writer.Write((byte) RecorderPacketType.Video);
                            var vp = (VideoPacket) packet;
                            writer.Write(vp.Timestamp);
                            writer.Write(vp.FrameNumber);
                            writer.Write(vp.Height);
                            writer.Write(vp.Width);
                            writer.Write((byte) vp.FrameType);
                            writer.Write(vp.Data.Length);
                            writer.Write(vp.Data);
                        }
                    }
                    Thread.Sleep(1);
                }
                writer.Flush();
            }
        }
    }
}