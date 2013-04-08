using System.Runtime.InteropServices;

namespace AR.Drone.Client.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct navdata_trims_t
    {
        public ushort tag;
        public ushort size;
        public float angular_rates_trim_r;
        public float euler_angles_trim_theta;
        public float euler_angles_trim_phi;
    }
}