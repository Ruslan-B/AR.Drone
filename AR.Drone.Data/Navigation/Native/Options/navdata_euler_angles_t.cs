using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_euler_angles_t
    {
        public ushort tag;
        public ushort size;
        public float theta_a;
        public float phi_a;
    }
}