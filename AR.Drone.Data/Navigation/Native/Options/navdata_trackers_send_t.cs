using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public unsafe struct navdata_trackers_send_t
    {
        public ushort tag;
        public ushort size;
        public fixed int locked [30]; // <Ctype "c_int32 * ((5 + 1) * (4 + 1))">
        // mapping incomplete
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        //public screen_point_t[] point; // <Ctype "screen_point_t * ((5 + 1) * (4 + 1))">
    }
}