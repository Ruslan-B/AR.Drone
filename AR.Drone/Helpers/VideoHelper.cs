using System;
using AR.Drone.Api.Video;
using AR.Drone.NativeApi.Video;
using FFmpeg.AutoGen;

namespace AR.Drone.Helpers
{
    public class VideoHelper
    {
        public static Native.AVPixelFormat Convert(VideoFramePixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case VideoFramePixelFormat.Gray8:
                    return Native.AVPixelFormat.PIX_FMT_GRAY8;
                case VideoFramePixelFormat.RGB24:
                    return Native.AVPixelFormat.PIX_FMT_BGR24;
                default:
                    throw new ArgumentOutOfRangeException("pixelFormat");
            }
        }

        public static FrameType Convert(VideoEncapsulationFrameType frameType)
        {
            switch (frameType)
            {
                case VideoEncapsulationFrameType.IDR:
                case VideoEncapsulationFrameType.I:
                    return FrameType.I;
                case VideoEncapsulationFrameType.P:
                    return FrameType.I;
                case VideoEncapsulationFrameType.Unknnown:
                case VideoEncapsulationFrameType.Headers:
                    return FrameType.Unknnown;
                default:
                    throw new ArgumentOutOfRangeException("frameType");
            }
        }
    }
}