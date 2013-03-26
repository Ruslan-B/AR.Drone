using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct NavdataCheckSum
    {
        public NavdataTag tag;
        public ushort size;
        public uint cks;
    }
}