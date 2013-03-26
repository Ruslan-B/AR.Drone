using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1,  CharSet = CharSet.Ansi)]
    public struct NavdataEulerAngles
    {
        public ushort tag;
        public ushort size;
        public float theta_a;
        public float phi_a;
    }
}