using System.Runtime.InteropServices;

namespace AR.Drone.Video
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VideoFrame
    {
        public long Timestamp;
        public uint Number;
        public int Width;
        public int Height;
        public int Depth;
        public PixelFormat PixelFormat;
        public byte[] Data;
    }
}