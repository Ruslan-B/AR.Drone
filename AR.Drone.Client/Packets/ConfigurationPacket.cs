using System.Runtime.InteropServices;

namespace AR.Drone.Client.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ConfigurationPacket
    {
        public byte[] Data;
        public long Timestamp;
    }
}