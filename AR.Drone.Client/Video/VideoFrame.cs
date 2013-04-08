using System.Runtime.InteropServices;

namespace AR.Drone.Client.Video
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VideoFrame
    {
        public long Timestamp;
        public uint FrameNumber;
        public VideoFramePixelFormat PixelFormat;
        public byte[,,] Data;
    }
}