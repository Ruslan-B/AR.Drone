using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct NavdataOption
    {
        public NavdataTag tag;
        public ushort size;
        // public byte[] data; -- it is not posible to map raw data 
    }
}