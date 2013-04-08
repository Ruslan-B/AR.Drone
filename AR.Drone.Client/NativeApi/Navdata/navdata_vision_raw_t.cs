using System.Runtime.InteropServices;

namespace AR.Drone.Client.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_vision_raw_t
    {
        public ushort tag;
        public ushort size;
        public float vision_tx_raw;
        public float vision_ty_raw;
        public float vision_tz_raw;
    }
}