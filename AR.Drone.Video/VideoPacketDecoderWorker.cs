using System;
using System.Collections.Concurrent;
using System.Threading;
using AR.Drone.Infrastructure;
using AR.Drone.Data;

namespace AR.Drone.Video
{
    public class VideoPacketDecoderWorker : WorkerBase
    {
        private bool _skipFrames;
        private readonly Action<VideoFrame> _onFrameDecoded;
        private readonly ConcurrentQueue<VideoPacket> _packetQueue;
        private readonly PixelFormat _pixelFormat;

        public VideoPacketDecoderWorker(PixelFormat pixelFormat, bool skipFrames, Action<VideoFrame> onFrameDecoded)
        {
            _pixelFormat = pixelFormat;
            _skipFrames = skipFrames;
            _onFrameDecoded = onFrameDecoded;
            _packetQueue = new ConcurrentQueue<VideoPacket>();
        }

        public void EnqueuePacket(VideoPacket packet)
        {
            if (_skipFrames && packet.FrameType == VideoFrameType.I && _packetQueue.Count > 0)
            {
                _packetQueue.Flush();
            }
            _packetQueue.Enqueue(packet);
        }

        protected override void Loop(CancellationToken token)
        {
            // flush packet queue
            _packetQueue.Flush();

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