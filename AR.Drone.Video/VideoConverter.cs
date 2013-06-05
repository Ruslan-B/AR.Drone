using AR.Drone.Infrastructure;
using AR.Drone.Video.Exceptions;
using FFmpeg.AutoGen;

namespace AR.Drone.Video
{
    public unsafe class VideoConverter : DisposableBase
    {
        private readonly AVPixelFormat _pixelFormat;
        private bool _initialized;

        private byte[] _outputData;

        private SwsContext* _pContext;
        private AVFrame* _pCurrentFrame;


        public VideoConverter(AVPixelFormat pixelFormat)
        {
            _pixelFormat = pixelFormat;
        }

        private void Initialize(int width, int height, AVPixelFormat inFormat)
        {
            _initialized = true;

            _pContext = FFmpegInvoke.sws_getContext(width, height, inFormat,
                                                    width, height, _pixelFormat,
                                                    FFmpegInvoke.SWS_FAST_BILINEAR, null, null, null);
            if (_pContext == null)
                throw new VideoConverterException("Could not initialize the conversion context.");

            _pCurrentFrame = FFmpegInvoke.avcodec_alloc_frame();

            int outputDataSize = FFmpegInvoke.avpicture_get_size(_pixelFormat, width, height);
            _outputData = new byte[outputDataSize];

            fixed (byte* pOutputData = &_outputData[0])
            {
                FFmpegInvoke.avpicture_fill((AVPicture*) _pCurrentFrame, pOutputData, _pixelFormat, width, height);
            }
        }

        public byte[] ConvertFrame(ref AVFrame frame)
        {
            if (_initialized == false)
                Initialize(frame.width, frame.height, (AVPixelFormat) frame.format);

            fixed (AVFrame* pFrame = &frame)
            fixed (byte* pOutputData = &_outputData[0])
            {
                byte** pSrcData = &(pFrame)->data_0;
                byte** pDstData = &(_pCurrentFrame)->data_0;

                _pCurrentFrame->data_0 = pOutputData;
                FFmpegInvoke.sws_scale(_pContext, pSrcData, pFrame->linesize, 0, frame.height, pDstData, _pCurrentFrame->linesize);
            }
            return _outputData;
        }

        protected override void DisposeOverride()
        {
            if (_initialized == false) return;

            FFmpegInvoke.sws_freeContext(_pContext);
            FFmpegInvoke.av_free(_pCurrentFrame);
        }
    }
}