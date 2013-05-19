using System.Diagnostics;
using AI.Core.System;
using AR.Drone.Client.Video.Exceptions;
using FFmpeg.AutoGen;

namespace AR.Drone.Client.Video.FFmpeg
{
    public unsafe class VideoDecoder : DisposableBase
    {
        private const AVCodecID CodecId = AVCodecID.AV_CODEC_ID_H264;
        private readonly AVCodecContext* _pDecodingContext;

        static VideoDecoder()
        {
            FFmpegInvoke.av_register_all();
            FFmpegInvoke.avcodec_register_all();
        }

        public VideoDecoder()
        {
            AVCodec* pCodec = FFmpegInvoke.avcodec_find_decoder(CodecId);
            if (pCodec == null)
                throw new VideoDecoderException("Unsupported codec.");

            _pDecodingContext = FFmpegInvoke.avcodec_alloc_context3(pCodec);

            if (FFmpegInvoke.avcodec_open2(_pDecodingContext, pCodec, null) < 0)
                throw new VideoDecoderException("Could not open codec.");
        }

        public bool TryDecode(ref byte[] data, out AVFrame frame)
        {
            int gotPicture;
            frame = new AVFrame();
            fixed (byte* pData = &data[0])
            fixed (AVFrame* pFrame = &frame)
            {
                var packet = new AVPacket {data = pData, size = data.Length};
                int decodedSize = FFmpegInvoke.avcodec_decode_video2(_pDecodingContext, pFrame, &gotPicture, &packet);
                if (decodedSize < 0)
                    Trace.TraceWarning("Error while decoding frame.");
            }
            return gotPicture == 1;
        }


        protected override void DisposeOverride()
        {
            FFmpegInvoke.avcodec_close(_pDecodingContext);
        }
    }
}