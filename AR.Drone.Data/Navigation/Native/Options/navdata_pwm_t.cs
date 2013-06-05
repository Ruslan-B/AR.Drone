using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_pwm_t
    {
        public ushort tag;
        public ushort size;
        public byte motor1;
        public byte motor2;
        public byte motor3;
        public byte motor4;
        public byte sat_motor1;
        public byte sat_motor2;
        public byte sat_motor3;
        public byte sat_motor4;
        public float gaz_feed_forward;
        public float gaz_altitude;
        public float altitude_integral;
        public float vz_ref;
        public int u_pitch;
        public int u_roll;
        public int u_yaw;
        public float yaw_u_I;
        public int u_pitch_planif;
        public int u_roll_planif;
        public int u_yaw_planif;
        public float u_gaz_planif;
        public ushort current_motor1;
        public ushort current_motor2;
        public ushort current_motor3;
        public ushort current_motor4;
        public float altitude_prop;
        public float altitude_der;
    }
}