using System.Runtime.InteropServices;

namespace AR.Drone.Client.Video
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VideoPacket
    {
        public long Timestamp;
        public uint FrameNumber;
        public ushort Height;
        public ushort Width;
        public FrameType FrameType;
        public byte[] Data;
    }
}