using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct NavdataTime
    {
        public ushort tag;
        public ushort size;
        public uint time;
    }
}