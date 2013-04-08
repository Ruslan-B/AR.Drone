using System.Diagnostics;
using AI.Core.System;
using AR.Drone.Client.Video.Exceptions;
using FFmpeg.AutoGen;

namespace AR.Drone.Client.Video
{
    public unsafe class VideoDecoder : DisposableBase
    {
        private const FFmpegNative.AVCodecID CodecId = FFmpegNative.AVCodecID.AV_CODEC_ID_H264;
        private const FFmpegNative.AVPixelFormat PixelFormat = FFmpegNative.AVPixelFormat.PIX_FMT_YUV420P;

        private readonly int _height;
        private readonly int _width;

        private FFmpegNative.AVFrame* _pCurrentFrame;
        private FFmpegNative.AVCodecContext* _pDecodingContext;

        static VideoDecoder()
        {
            FFmpegNative.av_register_all();
            FFmpegNative.avcodec_register_all();
        }

        public VideoDecoder(int width, int height)
        {
            _width = width;
            _height = height;

            Initialize();
        }

        private void Initialize()
        {
            FFmpegNative.AVCodec* pCodec = FFmpegNative.avcodec_find_decoder(CodecId);
            if (pCodec == null)
                throw new VideoDecoderException("Unsupported codec.");

            _pDecodingContext = FFmpegNative.avcodec_alloc_context3(pCodec);
            _pDecodingContext->width = _width;
            _pDecodingContext->height = _height;
            _pDecodingContext->pix_fmt = PixelFormat;

            if (FFmpegNative.avcodec_open2(_pDecodingContext, pCodec, null) < 0)
                throw new VideoDecoderException("Could not open codec.");


            _pCurrentFrame = FFmpegNative.avcodec_alloc_frame();
        }

        public bool TryDecode(ref byte[] data, out FFmpegNative.AVFrame frame)
        {
            int gotPicture;
            fixed (byte* pData = &data[0])
            {
                var packet = new FFmpegNative.AVPacket {data = pData, size = data.Length};
                int decodedSize = FFmpegNative.avcodec_decode_video2(_pDecodingContext, _pCurrentFrame, &gotPicture, &packet);
                if (decodedSize < 0)
                    Trace.TraceWarning("Error while decoding frame.");
            }
            frame = *_pCurrentFrame;
            return gotPicture == 1;
        }


        protected override void DisposeOverride()
        {
            FFmpegNative.avcodec_close(_pDecodingContext);
            FFmpegNative.av_free(_pCurrentFrame);
        }
    }
}