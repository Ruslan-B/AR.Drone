using System.Runtime.InteropServices;
using AR.Drone.Client.Video;

namespace AR.Drone.Client.Packets
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