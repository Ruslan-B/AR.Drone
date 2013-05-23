using System.Runtime.InteropServices;

namespace AR.Drone.Client.Acquisition.Video.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public unsafe struct parrot_video_encapsulation_t
    {
        public fixed byte signature [4]; // <Ctype "c_uint8 * 4">
        public byte version;
        public byte video_codec;
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
        public byte frame_type;
        public byte control;
        public uint stream_byte_position_lw;
        public uint stream_byte_position_uw;
        public ushort stream_id;
        public byte total_slices;
        public byte slice_index;
        public byte header1_size;
        public byte header2_size;
        public fixed byte reserved2 [2]; // <Ctype "c_uint8 * 2">
        public uint advertised_size;
        public fixed byte reserved3 [12]; // <Ctype "c_uint8 * 12">
    }
}