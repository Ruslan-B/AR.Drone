using AI.Core.System;
using AR.Drone.Client.Packets;
using FFmpeg.AutoGen;

namespace AR.Drone.Video
{
    public class VideoPacketDecoder : DisposableBase
    {
        private readonly PixelFormat _pixelFormat;
        private VideoConverter _videoConverter;
        private VideoDecoder _videoDecoder;

        public VideoPacketDecoder(PixelFormat pixelFormat)
        {
            _pixelFormat = pixelFormat;
        }

        public bool TryDecode(ref VideoPacket packet, out VideoFrame frame)
        {
            if (_videoDecoder == null)
                _videoDecoder = new VideoDecoder();

            frame = new VideoFrame();
            AVFrame avFrame;
            if (_videoDecoder.TryDecode(ref packet.Data, out avFrame))
            {
                if (_videoConverter == null)
                    _videoConverter = new VideoConverter(_pixelFormat.ToAVPixelFormat());

                frame.Timestamp = packet.Timestamp;
                frame.FrameNumber = packet.FrameNumber;
                frame.Width = packet.Width;
                frame.Height = packet.Height;
                frame.PixelFormat = _pixelFormat;
                frame.Data = _videoConverter.ConvertFrame(avFrame);

                return true;
            }
            return false;
        }


        protected override void DisposeOverride()
        {
            if (_videoDecoder != null)
                _videoDecoder.Dispose();
            if (_videoConverter != null)
                _videoConverter.Dispose();
        }
    }
}