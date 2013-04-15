using System.Runtime.InteropServices;

namespace AR.Drone.Client.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NavigationPacket
    {
        public long Timestamp;
        public byte[] Data;
    }
}