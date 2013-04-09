using System.Diagnostics;
using AI.Core.System;
using AR.Drone.Client.Video.Exceptions;
using FFmpeg.AutoGen;

namespace AR.Drone.Client.Video
{
    public unsafe class VideoDecoder : DisposableBase
    {
        private const FFmpegNative.AVCodecID CodecId = FFmpegNative.AVCodecID.AV_CODEC_ID_H264;
        private FFmpegNative.AVCodecContext* _pDecodingContext;

        static VideoDecoder()
        {
            FFmpegNative.av_register_all();
            FFmpegNative.avcodec_register_all();
        }

        public VideoDecoder()
        {
            Initialize();
        }

        private void Initialize()
        {
            FFmpegNative.AVCodec* pCodec = FFmpegNative.avcodec_find_decoder(CodecId);
            if (pCodec == null)
                throw new VideoDecoderException("Unsupported codec.");

            _pDecodingContext = FFmpegNative.avcodec_alloc_context3(pCodec);

            if (FFmpegNative.avcodec_open2(_pDecodingContext, pCodec, null) < 0)
                throw new VideoDecoderException("Could not open codec.");
        }

        // todo out VideoFrame
        public bool TryDecode(ref byte[] data, out FFmpegNative.AVFrame frame)
        {
            int gotPicture;
            frame = new FFmpegNative.AVFrame();
            fixed (byte* pData = &data[0])
            fixed (FFmpegNative.AVFrame* pFrame = &frame)
            {
                var packet = new FFmpegNative.AVPacket {data = pData, size = data.Length};
                int decodedSize = FFmpegNative.avcodec_decode_video2(_pDecodingContext, pFrame, &gotPicture, &packet);
                if (decodedSize < 0)
                    Trace.TraceWarning("Error while decoding frame.");
            }
            return gotPicture == 1;
        }


        protected override void DisposeOverride()
        {
            FFmpegNative.avcodec_close(_pDecodingContext);
        }
    }
}