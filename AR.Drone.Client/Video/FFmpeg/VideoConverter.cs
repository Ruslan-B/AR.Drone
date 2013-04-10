using AI.Core.System;
using AR.Drone.Client.Video.Exceptions;
using FFmpeg.AutoGen;

namespace AR.Drone.Client.Video.FFmpeg
{
    public unsafe class VideoConverter : DisposableBase
    {
        private readonly FFmpegNative.AVPixelFormat _pixelFormat;
        private bool _initialized;

        private byte[,,] _outputData;

        private FFmpegNative.SwsContext* _pContext;
        private FFmpegNative.AVFrame* _pCurrentFrame;


        public VideoConverter(FFmpegNative.AVPixelFormat pixelFormat)
        {
            _pixelFormat = pixelFormat;
        }

        private void Initialize(int width, int height, FFmpegNative.AVPixelFormat inFormat)
        {
            _initialized = true;

            _pContext = FFmpegNative.sws_getContext(width, height, inFormat,
                                                    width, height, _pixelFormat,
                                                    FFmpegNative.SWS_FAST_BILINEAR, null, null, null);
            if (_pContext == null)
                throw new VideoConverterException("Could not initialize the conversion context.");

            _pCurrentFrame = FFmpegNative.avcodec_alloc_frame();

            int outputDataSize = FFmpegNative.avpicture_get_size(_pixelFormat, width, height);
            int depth = outputDataSize/(height*width);
            _outputData = new byte[height,width,depth];

            fixed (byte* pOutputData = &_outputData[0, 0, 0])
            {
                FFmpegNative.avpicture_fill((FFmpegNative.AVPicture*) _pCurrentFrame, pOutputData, _pixelFormat, width, height);
            }
        }

        public byte[,,] ConvertFrame(FFmpegNative.AVFrame frame)
        {
            if (_initialized == false)
                Initialize(frame.width, frame.height, (FFmpegNative.AVPixelFormat) frame.format);

            fixed (byte* pOutputData = &_outputData[0, 0, 0])
            {
                byte** pSrcData = &(frame).data_0;
                byte** pDstData = &(_pCurrentFrame)->data_0;

                _pCurrentFrame->data_0 = pOutputData;
                FFmpegNative.sws_scale(_pContext, pSrcData, frame.linesize, 0, frame.height, pDstData, _pCurrentFrame->linesize);
            }
            return _outputData;
        }

        protected override void DisposeOverride()
        {
            if (_initialized == false) return;

            FFmpegNative.sws_freeContext(_pContext);
            FFmpegNative.av_free(_pCurrentFrame);
        }
    }
}