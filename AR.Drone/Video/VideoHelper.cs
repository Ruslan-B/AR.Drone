using System;
using FFmpeg.AutoGen;

namespace AR.Drone.Video
{
    public class VideoHelper
    {
        public static FFmpegNative.AVPixelFormat Convert(VideoFramePixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case VideoFramePixelFormat.Gray8:
                    return FFmpegNative.AVPixelFormat.PIX_FMT_GRAY8;
                case VideoFramePixelFormat.RGB24:
                    return FFmpegNative.AVPixelFormat.PIX_FMT_BGR24;
                default:
                    throw new ArgumentOutOfRangeException("pixelFormat");
            }
        }
    }
}