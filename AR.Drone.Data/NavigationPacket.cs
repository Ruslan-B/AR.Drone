using System.Runtime.InteropServices;

namespace AR.Drone.Data
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NavigationPacket
    {
        public long Timestamp;
        public byte[] Data;
    }
}