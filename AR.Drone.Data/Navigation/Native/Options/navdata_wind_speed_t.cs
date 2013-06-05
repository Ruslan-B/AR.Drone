using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_wind_speed_t
    {
        public ushort tag;
        public ushort size;
        public float wind_speed;
        public float wind_angle;
        public float wind_compensation_theta;
        public float wind_compensation_phi;
        public float state_x1;
        public float state_x2;
        public float state_x3;
        public float state_x4;
        public float state_x5;
        public float state_x6;
        public float magneto_debug1;
        public float magneto_debug2;
        public float magneto_debug3;
    }
}