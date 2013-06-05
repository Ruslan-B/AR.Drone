using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_vision_t
    {
        public ushort tag;
        public ushort size;
        public uint vision_state;
        public int vision_misc;
        public float vision_phi_trim;
        public float vision_phi_ref_prop;
        public float vision_theta_trim;
        public float vision_theta_ref_prop;
        public int new_raw_picture;
        public float theta_capture;
        public float phi_capture;
        public float psi_capture;
        public int altitude_capture;
        public uint time_capture;
        public velocities_t body_v;
        public float delta_phi;
        public float delta_theta;
        public float delta_psi;
        public uint gold_defined;
        public uint gold_reset;
        public float gold_x;
        public float gold_y;
    }
}