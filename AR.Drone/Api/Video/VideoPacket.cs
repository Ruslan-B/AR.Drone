using System;
using System.Runtime.InteropServices;

namespace AR.Drone.Api.Video
{
    [StructLayout(LayoutKind.Sequential)]
    public class VideoPacket
    {
        public DateTime Timestamp;
        public uint FrameNumber;
        public int Height;
        public int Width;
        public FrameType FrameType;
        public byte[] Data;
    }
}