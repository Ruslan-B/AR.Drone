using System;
using FFmpeg.AutoGen;

namespace AR.Drone.Video
{
    public static class PixelFormatConverter
    {
        public static AVPixelFormat ToAVPixelFormat(this PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Gray8:
                    return AVPixelFormat.PIX_FMT_GRAY8;
                case PixelFormat.BGR24:
                    return AVPixelFormat.PIX_FMT_BGR24;
                case PixelFormat.RGB24:
                    return AVPixelFormat.PIX_FMT_RGB24;
                default:
                    throw new ArgumentOutOfRangeException("pixelFormat");
            }
        }
    }
}