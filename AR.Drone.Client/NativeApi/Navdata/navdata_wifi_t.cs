using System.Runtime.InteropServices;

namespace AR.Drone.Client.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_wifi_t
    {
        public ushort tag;
        public ushort size;
        public uint link_quality;
    }
}