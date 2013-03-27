using System;
using System.Runtime.InteropServices;

namespace AR.Drone.Api.Video
{
    [StructLayout(LayoutKind.Sequential)]
    public class VideoFrame
    {
        public DateTime Timestamp;
        public uint FrameNumber;
        public int Height;
        public int Width;
        public int Depth;
        public VideoFramePixelFormat PixelFormat;
        public byte[,,] Data;
    }
}