using System;
using FFmpeg.AutoGen;

namespace AR.Drone.Client.Video.FFmpeg
{
    public class PixelFormatHelper
    {
        public static FFmpegNative.AVPixelFormat ToAVPixelFormat(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Gray8:
                    return FFmpegNative.AVPixelFormat.PIX_FMT_GRAY8;
                case PixelFormat.BGR24:
                    return FFmpegNative.AVPixelFormat.PIX_FMT_BGR24;
                case PixelFormat.RGB24:
                    return FFmpegNative.AVPixelFormat.PIX_FMT_RGB24;
                default:
                    throw new ArgumentOutOfRangeException("pixelFormat");
            }
        }
    }
}