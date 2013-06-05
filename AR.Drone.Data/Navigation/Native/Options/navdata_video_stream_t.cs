using System.Runtime.InteropServices;

namespace AR.Drone.Data.Navigation.Native.Options
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct navdata_video_stream_t
    {
        public ushort tag;
        public ushort size;
        public byte quant;
        public uint frame_size;
        public uint frame_number;
        public uint atcmd_ref_seq;
        public uint atcmd_mean_ref_gap;
        public float atcmd_var_ref_gap;
        public uint atcmd_ref_quality;
        public uint out_bitrate;
        public uint desired_bitrate;
        public int data1;
        public int data2;
        public int data3;
        public int data4;
        public int data5;
        public uint tcp_queue_level;
        public uint fifo_queue_level;
    }
}