using System.Runtime.InteropServices;

namespace AR.Drone.Data
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ConfigurationPacket
    {
        public byte[] Data;
        public long Timestamp;
    }
}