using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi.Math
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Vector2
    {
        public float x; 
        public float y; 
    }
}