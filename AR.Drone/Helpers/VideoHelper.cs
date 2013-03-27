using System;
using AR.Drone.Api.Video;
using AR.Drone.NativeApi;
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

        public static FrameType Convert(parrot_video_encapsulation_frametypes_t frameType)
        {
            switch (frameType)
            {
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_IDR_FRAME:
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_I_FRAME:
                    return FrameType.I;
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_P_FRAME:
                    return FrameType.I;
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_UNKNNOWN:
                case parrot_video_encapsulation_frametypes_t.FRAME_TYPE_HEADERS:
                    return FrameType.Unknnown;
                default:
                    throw new ArgumentOutOfRangeException("frameType");
            }
        }
    }
}