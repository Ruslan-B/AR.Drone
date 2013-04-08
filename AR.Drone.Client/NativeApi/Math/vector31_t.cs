using System.Runtime.InteropServices;

namespace AR.Drone.Client.NativeApi.Math
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct vector31_t
    {
        public float x;
        public float y;
        public float z;
    }
}