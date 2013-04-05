using System.Diagnostics;
using AR.Drone.Common;
using AR.Drone.Video.Exceptions;
using FFmpeg.AutoGen;

namespace AR.Drone.Video
{
    public unsafe class VideoDecoder : DisposableBase
    {
        private const Native.AVCodecID CodecId = Native.AVCodecID.AV_CODEC_ID_H264;
        private const Native.AVPixelFormat PixelFormat = Native.AVPixelFormat.PIX_FMT_YUV420P;

        private readonly int _height;
        private readonly int _width;

        private bool _initialized;

        private Native.AVFrame* _pCurrentFrame;
        private Native.AVCodecContext* _pDecodingContext;

        static VideoDecoder()
        {
            Native.av_register_all();
            Native.avcodec_register_all();
        }

        public VideoDecoder(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Initialize()
        {
            if (_initialized) throw new VideoDecoderException("Video decoder already Initialized.");
            _initialized = true;

            Native.AVCodec* pCodec = Native.avcodec_find_decoder(CodecId);
            if (pCodec == null)
                throw new VideoDecoderException("Unsupported codec.");

            _pDecodingContext = Native.avcodec_alloc_context3(pCodec);
            _pDecodingContext->width = _width;
            _pDecodingContext->height = _height;
            _pDecodingContext->pix_fmt = PixelFormat;

            if (Native.avcodec_open2(_pDecodingContext, pCodec, null) < 0)
                throw new VideoDecoderException("Could not open codec.");


            _pCurrentFrame = Native.avcodec_alloc_frame();
        }

        public bool TryDecode(ref byte[] data, out Native.AVFrame frame)
        {
            int gotPicture;
            fixed (byte* pData = &data[0])
            {
                var packet = new Native.AVPacket {data = pData, size = data.Length};
                int decodedSize = Native.avcodec_decode_video2(_pDecodingContext, _pCurrentFrame, &gotPicture, &packet);
                if (decodedSize < 0)
                    Trace.TraceWarning("Error while decoding frame");
            }
            frame = *_pCurrentFrame;
            return gotPicture == 1;
        }


        protected override void DisposeOverride()
        {
            if (_initialized)
            {
                Native.avcodec_close(_pDecodingContext);
                Native.av_free(_pCurrentFrame);
            }
        }
    }
}