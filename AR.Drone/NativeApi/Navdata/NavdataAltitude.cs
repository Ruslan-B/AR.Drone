using System.Runtime.InteropServices;
using AR.Drone.NativeApi.Math;

namespace AR.Drone.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct NavdataAltitude
    {
        public ushort tag;
        public ushort size;
        public int altitude_vision;
        public float altitude_vz;
        public int altitude_ref;
        public int altitude_raw;
        public float obs_accZ;
        public float obs_alt;
        public Vector3 obs_x;
        public uint obs_state;
        public Vector2 est_vb;
        public uint est_state;
    }
}