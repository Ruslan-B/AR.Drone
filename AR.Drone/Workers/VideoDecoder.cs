using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using AR.Drone.Exceptions;
using AR.Drone.Helpers;
using AR.Drone.Api.Video;
using AR.Drone.Common;
using FFmpeg.AutoGen;

namespace AR.Drone.Workers
{
    public class VideoDecoder : WorkerBase
    {
        private readonly ConcurrentQueue<VideoPacket> _packetQueue;

        static VideoDecoder()
        {
            Native.av_register_all();
            Native.avcodec_register_all();
        }

        public VideoDecoder()
        {
            _packetQueue = new ConcurrentQueue<VideoPacket>();
            Width = 640;
            Height = 360;
            OutputPixelFormat = VideoFramePixelFormat.RGB24;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public VideoFramePixelFormat OutputPixelFormat { get; set; }

        public event Action<VideoDecoder, VideoFrame> FrameDecoded;

        public void EnqueuePacket(VideoPacket packet)
        {
            _packetQueue.Enqueue(packet);
        }

        protected override unsafe void Loop(CancellationToken token)
        {
            // fixing staring state, all structures will be creating using this values
            VideoFramePixelFormat outputPixelFormat = OutputPixelFormat;
            int width = Width;
            int height = Height;
            // flush packet queue
            ConcurrentQueueHelper.Clear(_packetQueue);
            // create output picture 
            Native.AVPixelFormat convertToPixelFormat = VideoHelper.Convert(outputPixelFormat);
            int outputDataSize = Native.avpicture_get_size(convertToPixelFormat, width, height);
            int depth = outputDataSize/(height*width);
            var outputData = new byte[height,width,depth];
            fixed (byte* pOutputData = &outputData[0, 0, 0])
            {
                Native.AVFrame* pDecodedFrame = Native.avcodec_alloc_frame();
                Native.AVFrame* pConvertedFrame = Native.avcodec_alloc_frame();

                Native.avpicture_fill((Native.AVPicture*) pConvertedFrame, pOutputData, convertToPixelFormat, width, height);

                Native.SwsContext* pConversionContext = null;
                Native.AVCodecContext* pDecodingContext = null;

                bool initialized = false;
                while (token.IsCancellationRequested == false)
                {
                    VideoPacket packet;
                    if (_packetQueue.TryDequeue(out packet))
                    {
                        if (initialized == false)
                        {
                            pDecodingContext = CreateDecodingContext(packet.Width, packet.Height);
                            pConversionContext = CreateConversionContext(pDecodingContext, width, height, convertToPixelFormat);
                            initialized = true;
                        }

                        bool decoded = DecodeVideo(pDecodingContext, pDecodedFrame, packet);

                        if (decoded)
                        {
                            ConvertFrame(pConversionContext, pDecodedFrame, pConvertedFrame);
                            var frame = new VideoFrame
                                {
                                    Timestamp = packet.Timestamp,
                                    Width = width,
                                    Height = height,
                                    Depth = depth,
                                    PixelFormat = outputPixelFormat,
                                    Data = outputData
                                };
                            OnFrameDecoded(frame);
                        }
                    }
                    Thread.Sleep(10);
                }

                if (initialized)
                {
                    Native.avcodec_close(pDecodingContext);
                    Native.sws_freeContext(pConversionContext);
                }
                Native.av_free(pDecodedFrame);
                Native.av_free(pConvertedFrame);
            }
        }

        private unsafe Native.AVCodecContext* CreateDecodingContext(int width, int height)
        {
            Native.AVCodec* pCodec = Native.avcodec_find_decoder(Native.AVCodecID.AV_CODEC_ID_H264);
            if (pCodec == null)
                throw new VideoDecoderException("Unsupported codec");

            Native.AVCodecContext* pDecodingContext = Native.avcodec_alloc_context3(pCodec);
            pDecodingContext->width = width;
            pDecodingContext->height = height;
            pDecodingContext->pix_fmt = Native.AVPixelFormat.PIX_FMT_YUV420P;

            if (Native.avcodec_open2(pDecodingContext, pCodec, null) < 0)
                throw new VideoDecoderException("Could not open codec");

            return pDecodingContext;
        }

        private unsafe Native.SwsContext* CreateConversionContext(Native.AVCodecContext* pDecodingContext, int width, int height, Native.AVPixelFormat convertToPixelFormat)
        {
            Native.SwsContext* pConversionContext = Native.sws_getContext(pDecodingContext->width, pDecodingContext->height, pDecodingContext->pix_fmt,
                                                                          width, height, convertToPixelFormat,
                                                                          Native.SWS_FAST_BILINEAR, null, null, null);
            if (pConversionContext == null)
                throw new VideoDecoderException("Could not initialize the conversion context");

            return pConversionContext;
        }

        private unsafe bool DecodeVideo(Native.AVCodecContext* pDecodingContext, Native.AVFrame* pDecodedFrame, VideoPacket videoPacket)
        {
            int gotPicture;
            fixed (byte* pData = &videoPacket.Data[0])
            {
                var packet = new Native.AVPacket {data = pData, size = videoPacket.Data.Length};
                int decodedSize = Native.avcodec_decode_video2(pDecodingContext, pDecodedFrame, &gotPicture, &packet);
                if (decodedSize < 0)
                    Trace.TraceWarning("Error while decoding frame");
            }
            return gotPicture == 1;
        }

        private unsafe void ConvertFrame(Native.SwsContext* pContext, Native.AVFrame* pSrcFrame, Native.AVFrame* pDstFrame)
        {
            var pSrcData = (byte**) (&(*pSrcFrame).data[0]);
            var pDstData = (byte**) (&(*pDstFrame).data[0]);

            Native.sws_scale(pContext, pSrcData, pSrcFrame->linesize, 0, pSrcFrame->height, pDstData, pDstFrame->linesize);
        }

        private void OnFrameDecoded(VideoFrame frame)
        {
            if (FrameDecoded != null)
                FrameDecoded(this, frame);
        }
    }
}