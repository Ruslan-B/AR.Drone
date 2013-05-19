using System;
using System.Collections.Concurrent;
using System.Threading;
using AI.Core.System;
using AR.Drone.Client.Helpers;
using AR.Drone.Client.Packets;
using AR.Drone.Client.Video;

namespace AR.Drone.Client.Workers
{
    public class VideoPacketDecoderWorker : WorkerBase
    {
        private readonly PixelFormat _pixelFormat;
        private readonly Action<VideoFrame> _onFrameDecoded;
        private readonly ConcurrentQueue<VideoPacket> _packetQueue;

        public VideoPacketDecoderWorker(PixelFormat pixelFormat, Action<VideoFrame> onFrameDecoded)
        {
            _pixelFormat = pixelFormat;
            _onFrameDecoded = onFrameDecoded;
            _packetQueue = new ConcurrentQueue<VideoPacket>();
        }

        public void EnqueuePacket(VideoPacket packet)
        {
            _packetQueue.Enqueue(packet);
        }

        protected override void Loop(CancellationToken token)
        {
            // flush packet queue
            ConcurrentQueueHelper.Flush(_packetQueue);

            using (var videoDecoder = new VideoPacketDecoder(_pixelFormat))
                while (token.IsCancellationRequested == false)
                {
                    VideoPacket packet;
                    if (_packetQueue.TryDequeue(out packet))
                    {
                        VideoFrame frame;
                        if (videoDecoder.TryDecode(ref packet, out frame))
                        {
                            _onFrameDecoded(frame);
                        }
                    }
                    Thread.Sleep(10);
                }
        }
    }
}