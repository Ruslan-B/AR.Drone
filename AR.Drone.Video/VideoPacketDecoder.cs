using AR.Drone.Infrastructure;
using AR.Drone.Data;
using FFmpeg.AutoGen;

namespace AR.Drone.Video
{
    public class VideoPacketDecoder : DisposableBase
    {
        private readonly PixelFormat _pixelFormat;
        private VideoConverter _videoConverter;
        private VideoDecoder _videoDecoder;
        private AVFrame _avFrame;
        private AVPacket _avPacket;

        public VideoPacketDecoder(PixelFormat pixelFormat)
        {
            _pixelFormat = pixelFormat;
            _avFrame = new AVFrame();
            _avPacket = new AVPacket();
        }

        public unsafe bool TryDecode(ref VideoPacket packet, out VideoFrame frame)
        {
            if (_videoDecoder == null) _videoDecoder = new VideoDecoder();


            fixed (byte* pData = &packet.Data[0])
            {
                _avPacket.data = pData;
                _avPacket.size = packet.Data.Length;
                frame = new VideoFrame();
                if (_videoDecoder.TryDecode(ref _avPacket, ref _avFrame))
                {
                    if (_videoConverter == null) _videoConverter = new VideoConverter(_pixelFormat.ToAVPixelFormat());

                    byte[] data = _videoConverter.ConvertFrame(ref _avFrame);

                    frame.Timestamp = packet.Timestamp;
                    frame.Number = packet.FrameNumber;
                    frame.Width = packet.Width;
                    frame.Height = packet.Height;
                    frame.Depth = data.Length/(packet.Width*packet.Height);
                    frame.PixelFormat = _pixelFormat;
                    frame.Data = data;

                    return true;
                }
            }
            return false;
        }


        protected override void DisposeOverride()
        {
            if (_videoDecoder != null) _videoDecoder.Dispose();
            if (_videoConverter != null) _videoConverter.Dispose();
        }
    }
}