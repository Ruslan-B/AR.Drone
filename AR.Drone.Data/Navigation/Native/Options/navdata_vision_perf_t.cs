using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public unsafe struct navdata_vision_perf_t
    {
        public ushort tag;
        public ushort size;
        public float time_szo;
        public float time_corners;
        public float time_compute;
        public float time_tracking;
        public float time_trans;
        public float time_update;
        public fixed float time_custom [20]; // <Ctype "float32_t * 20">
    }
}