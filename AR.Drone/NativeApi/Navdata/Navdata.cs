using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi.Navdata
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct Navdata
    {
        public uint header;
        public ARDroneState ardrone_state;
        public uint sequence;
        public int vision_defined;
        //public NavdataOption options; -- it is not needed to map
    }
}