using System.Runtime.InteropServices;

namespace AR.Drone.Video
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VideoFrame
    {
        public long Timestamp;
        public uint FrameNumber;
        public int Width;
        public int Height;
        public PixelFormat PixelFormat;
        public byte[] Data;
    }
}