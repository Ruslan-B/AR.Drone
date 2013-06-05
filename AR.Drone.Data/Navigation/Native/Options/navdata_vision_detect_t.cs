using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public unsafe struct navdata_vision_detect_t
    {
        public ushort tag;
        public ushort size;
        public uint nb_detected;
        public fixed uint type [4]; // <Ctype "c_uint32 * 4">
        public fixed uint xc [4]; // <Ctype "c_uint32 * 4">
        public fixed uint yc [4]; // <Ctype "c_uint32 * 4">
        public fixed uint width [4]; // <Ctype "c_uint32 * 4">
        public fixed uint height [4]; // <Ctype "c_uint32 * 4">
        public fixed uint dist [4]; // <Ctype "c_uint32 * 4">
        public fixed float orientation_angle [4]; // <Ctype "float32_t * 4">
        // mapping incomplete
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //public matrix33_t[] rotation; // <Ctype "matrix33_t * 4">
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //public vector31_t[] translation; // <Ctype "vector31_t * 4">
        //public fixed uint camera_source[4]; // <Ctype "c_uint32 * 4">
    }
}