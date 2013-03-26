using System.Runtime.InteropServices;

namespace AR.Drone.NativeApi.Video
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public unsafe struct VideoEncapsulation
    {
        public fixed byte signature [4];
        public byte version;
        public VideoEncapsulationCodec video_codec;
        public ushort header_size;
        public uint payload_size;
        public ushort encoded_stream_width;
        public ushort encoded_stream_height;
        public ushort display_width;
        public ushort display_height;
        public uint frame_number;
        public uint timestamp;
        public byte total_chuncks;
        public byte chunck_index;
        public VideoEncapsulationFrameType frame_type;
        public VideoEncapsulationControl control;
        public uint stream_byte_position_lw;
        public uint stream_byte_position_uw;
        public ushort stream_id;
        public byte total_slices;
        public byte slice_index;
        public byte header1_size;
        public byte header2_size;
        public fixed byte reserved2 [2];
        public uint advertised_size;
        public fixed byte reserved3 [12];
    }
}