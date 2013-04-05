using AR.Drone.Common;
using AR.Drone.Helpers;
using AR.Drone.Video.Exceptions;
using FFmpeg.AutoGen;

namespace AR.Drone.Video
{
    public unsafe class VideoConverter : DisposableBase
    {
        private const Native.AVPixelFormat _inputPixelFormat = Native.AVPixelFormat.PIX_FMT_YUV420P;

        private readonly int _height;
        private readonly Native.AVPixelFormat _outputPixelFormat;
        private readonly int _width;

        private bool _initialized;
        private byte[,,] _outputData;

        private Native.SwsContext* _pContext;
        private Native.AVFrame* _pCurrentFrame;


        public VideoConverter(int width, int height, VideoFramePixelFormat pixelFormat)
        {
            _width = width;
            _height = height;
            _outputPixelFormat = VideoHelper.Convert(pixelFormat);
        }

        public void Initialize()
        {
            if (_initialized) throw new VideoConverterException("Video converter already Initialized.");

            _initialized = true;
            _pContext = Native.sws_getContext(_width, _height, _inputPixelFormat,
                                              _width, _height, _outputPixelFormat,
                                              Native.SWS_FAST_BILINEAR, null, null, null);
            if (_pContext == null)
                throw new VideoConverterException("Could not initialize the conversion context.");

            _pCurrentFrame = Native.avcodec_alloc_frame();

            int outputDataSize = Native.avpicture_get_size(_outputPixelFormat, _width, _height);
            int depth = outputDataSize/(_height*_width);
            _outputData = new byte[_height,_width,depth];

            fixed (byte* pOutputData = &_outputData[0, 0, 0])
            {
                Native.avpicture_fill((Native.AVPicture*) _pCurrentFrame, pOutputData, _outputPixelFormat, _width, _height);
            }
        }

        public byte[,,] ConvertFrame(Native.AVFrame srcFrame)
        {
            fixed (void* pOutputData = &_outputData[0, 0, 0])
            {
                var pSrcData = (byte**) (&(srcFrame).data[0]);
                var pDstData = (byte**) (&(*_pCurrentFrame).data[0]);

                _pCurrentFrame->data[0] = (int) pOutputData;
                Native.sws_scale(_pContext, pSrcData, srcFrame.linesize, 0, srcFrame.height, pDstData, _pCurrentFrame->linesize);
            }
            return _outputData;
        }

        protected override void DisposeOverride()
        {
            if (_initialized)
            {
                Native.sws_freeContext(_pContext);
                Native.av_free(_pCurrentFrame);
            }
        }
    }
}