using System;
using System.Collections.Concurrent;
using System.Threading;
using AR.Drone.Common;
using AR.Drone.Helpers;
using AR.Drone.Video;
using FFmpeg.AutoGen;

namespace AR.Drone.Workers
{
    public class VideoDecoderWorker : WorkerBase
    {
        private const int Width = 640;
        private const int Height = 360;
        private const VideoFramePixelFormat OutputPixelFormat = VideoFramePixelFormat.RGB24;

        private readonly Action<VideoFrame> _onFrameDecoded;
        private readonly ConcurrentQueue<VideoPacket> _packetQueue;

        public VideoDecoderWorker(Action<VideoFrame> onFrameDecoded)
        {
            _onFrameDecoded = onFrameDecoded;
            _packetQueue = new ConcurrentQueue<VideoPacket>();
        }

        //public VideoFramePixelFormat OutputPixelFormat { get; set; }

        public void EnqueuePacket(VideoPacket packet)
        {
            _packetQueue.Enqueue(packet);
        }

        protected override void Loop(CancellationToken token)
        {
            // flush packet queue
            ConcurrentQueueHelper.Flush(_packetQueue);
            // fixing staring state, all structures will be creating using this values

            bool initialized = false;
            using (var videoDecoder = new VideoDecoder(Width, Height))
            using (var videoConverter = new VideoConverter(Width, Height, OutputPixelFormat))
                while (token.IsCancellationRequested == false)
                {
                    VideoPacket packet;
                    if (_packetQueue.TryDequeue(out packet))
                    {
                        if (initialized == false)
                        {
                            initialized = true;
                            videoDecoder.Initialize();
                            videoConverter.Initialize();
                        }


                        Native.AVFrame decodedFrame;
                        if (videoDecoder.TryDecode(ref packet.Data, out decodedFrame))
                        {
                            byte[,,] decodedData = videoConverter.ConvertFrame(decodedFrame);
                            var frame = new VideoFrame
                                {
                                    Timestamp = packet.Timestamp,
                                    FrameNumber = packet.FrameNumber,
                                    PixelFormat = OutputPixelFormat,
                                    Data = decodedData
                                };
                            _onFrameDecoded(frame);
                        }
                    }
                    Thread.Sleep(10);
                }
        }
    }
}