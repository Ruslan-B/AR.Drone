using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_rc_references_t
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