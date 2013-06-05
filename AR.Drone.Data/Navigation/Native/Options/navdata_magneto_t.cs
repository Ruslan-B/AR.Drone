using System.Runtime.InteropServices;
using AR.Drone.Data.Navigation.Native.Math;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_magneto_t
    {
        public ushort tag;
        public ushort size;
        public short mx;
        public short my;
        public short mz;
        public vector31_t magneto_raw;
        public vector31_t magneto_rectified;
        public vector31_t magneto_offset;
        public float heading_unwrapped;
        public float heading_gyro_unwrapped;
        public float heading_fusion_unwrapped;
        public byte magneto_calibration_ok;
        public uint magneto_state;
        public float magneto_radius;
        public float error_mean;
        public float error_var;
    }
}