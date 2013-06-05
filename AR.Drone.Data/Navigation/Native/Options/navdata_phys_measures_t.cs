using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public unsafe struct navdata_phys_measures_t
    {
        public ushort tag;
        public ushort size;
        public float accs_temp;
        public ushort gyro_temp;
        public fixed float phys_accs [3]; // <Ctype "float32_t * NB_ACCS">
        public fixed float phys_gyros [3]; // <Ctype "float32_t * NB_GYROS">
        public uint alim3V3;
        public uint vrefEpson;
        public uint vrefIDG;
    }
}