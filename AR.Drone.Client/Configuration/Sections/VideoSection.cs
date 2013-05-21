using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class VideoSection
    {
        public readonly ReadOnlyItem<int> CamifFps = new ReadOnlyItem<int>("video:camif_fps");
        public readonly ReadWriteItem<int> CodecFps = new ReadWriteItem<int>("video:codec_fps");
        public readonly ReadOnlyItem<int> CamifBuffers = new ReadOnlyItem<int>("video:camif_buffers");
        public readonly ReadOnlyItem<int> Trackers = new ReadOnlyItem<int>("video:num_trackers");
        public readonly ReadWriteItem<VideoCodecType> Codec = new ReadWriteItem<VideoCodecType>("video:video_codec");
        public readonly ReadWriteItem<int> Slices = new ReadWriteItem<int>("video:video_slices");
        public readonly ReadWriteItem<int> LiveSocket = new ReadWriteItem<int>("video:video_live_socket");
        public readonly ReadOnlyItem<int> StorageSpace = new ReadOnlyItem<int>("video:video_storage_space");
        public readonly ReadWriteItem<int> Bitrate = new ReadWriteItem<int>("video:bitrate");
        public readonly ReadWriteItem<int> MaxBitrate = new ReadWriteItem<int>("video:max_bitrate");
        public readonly ReadWriteItem<VideoBitrateControlMode> BitrateCtrlMode = new ReadWriteItem<VideoBitrateControlMode>("video:bitrate_ctrl_mode");
        public readonly ReadWriteItem<int> BitrateStorage = new ReadWriteItem<int>("video:bitrate_storage");
        public readonly ReadWriteItem<VideoChannelType> Channel = new ReadWriteItem<VideoChannelType>("video:video_channel");
        public readonly ReadWriteItem<bool> OnUsb = new ReadWriteItem<bool>("video:video_on_usb");
        public readonly ReadWriteItem<int> FileIndex = new ReadWriteItem<int>("video:video_file_index");
    }
}