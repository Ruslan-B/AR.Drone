using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct NavdataRcReferences
    {
        public ushort tag;
        public ushort size;
        public int rc_ref_pitch;
        public int rc_ref_roll;
        public int rc_ref_yaw;
        public int rc_ref_gaz;
        public int rc_ref_ag;
    }
}