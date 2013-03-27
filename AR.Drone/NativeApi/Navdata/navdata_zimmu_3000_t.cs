using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_zimmu_3000_t
    {
        public ushort tag;
        public ushort size;
        public int vzimmuLSB;
        public float vzfind;
    }
}