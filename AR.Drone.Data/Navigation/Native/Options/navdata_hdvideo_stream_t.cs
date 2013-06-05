using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_hdvideo_stream_t
    {
        public ushort tag;
        public ushort size;
        public uint hdvideo_state;
        public uint storage_fifo_nb_packets;
        public uint storage_fifo_size;
        public uint usbkey_size;
        public uint usbkey_freespace;
        public uint frame_number;
        public uint usbkey_remaining_time;
    }
}