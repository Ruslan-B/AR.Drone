using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_references_t
    {
        public ushort tag;
        public ushort size;
        public int ref_theta;
        public int ref_phi;
        public int ref_theta_I;
        public int ref_phi_I;
        public int ref_pitch;
        public int ref_roll;
        public int ref_yaw;
        public int ref_psi;
        public float vx_ref;
        public float vy_ref;
        public float theta_mod;
        public float phi_mod;
        public float k_v_x;
        public float k_v_y;
        public uint k_mode;
        public float ui_time;
        public float ui_theta;
        public float ui_phi;
        public float ui_psi;
        public float ui_psi_accuracy;
        public int ui_seq;
    }
}