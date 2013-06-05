using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public unsafe struct navdata_vision_of_t
    {
        public ushort tag;
        public ushort size;
        public fixed float of_dx [5]; // <Ctype "float32_t * 5">
        public fixed float of_dy [5]; // <Ctype "float32_t * 5">
    }
}