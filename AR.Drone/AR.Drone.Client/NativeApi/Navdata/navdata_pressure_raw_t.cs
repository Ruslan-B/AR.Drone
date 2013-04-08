using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_pressure_raw_t
    {
        public ushort tag;
        public ushort size;
        public int up;
        public short ut;
        public int Temperature_meas;
        public int Pression_meas;
    }
}