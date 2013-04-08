using System.Runtime.InteropServices;

namespace AR.Drone.Client.Navigation
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NavigationPacket
    {
        public long Timestamp;
        public byte[] Data;
    }
}