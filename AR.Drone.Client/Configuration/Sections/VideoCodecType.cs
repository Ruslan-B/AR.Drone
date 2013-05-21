namespace AR.Drone.Client.Configuration.Sections
{
    public enum VideoCodecType
    {
        NULL = 0,
        UVLC = 0x20, // value is used for START_CODE
        MJPEG, // not used
        P263, // not used
        P264 = 0x40,
        MP4_360P = 0x80,
        H264_360P = 0x81,
        MP4_360P_H264_720P = 0x82,
        H264_720P = 0x83,
        MP4_360P_SLRS = 0x84,
        H264_360P_SLRS = 0x85,
        H264_720P_SLRS = 0x86,
        H264_AUTO_RESIZE = 0x87, // resolution is automatically adjusted according to bitrate
        MP4_360P_H264_360P = 0x88,
    }
}