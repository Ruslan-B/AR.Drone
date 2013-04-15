using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class VideoSection
    {
        public readonly ReadOnlyItem<int> CamifFps = new ReadOnlyItem<int>("video:camif_fps");
        public readonly ReadOnlyItem<int> CodecFps = new ReadOnlyItem<int>("video:codec_fps");
        public readonly ReadOnlyItem<int> CamifBuffers = new ReadOnlyItem<int>("video:camif_buffers");
        public readonly ReadOnlyItem<int> Trackers = new ReadOnlyItem<int>("video:num_trackers");
        public readonly ReadOnlyItem<int> Codec = new ReadOnlyItem<int>("video:video_codec");
        public readonly ReadOnlyItem<int> Slices = new ReadOnlyItem<int>("video:video_slices");
        public readonly ReadOnlyItem<int> LiveSocket = new ReadOnlyItem<int>("video:video_live_socket");
        public readonly ReadOnlyItem<int> StorageSpace = new ReadOnlyItem<int>("video:video_storage_space");
        public readonly ReadOnlyItem<int> Bitrate = new ReadOnlyItem<int>("video:bitrate");
        public readonly ReadOnlyItem<int> MaxBitrate = new ReadOnlyItem<int>("video:max_bitrate");
        public readonly ReadOnlyItem<int> BitrateCtrlMode = new ReadOnlyItem<int>("video:bitrate_ctrl_mode");
        public readonly ReadOnlyItem<int> BitrateStorage = new ReadOnlyItem<int>("video:bitrate_storage");
        public readonly ReadWriteItem<VideoChannelType> Channel = new ReadWriteItem<VideoChannelType>("video:video_channel");
        public readonly ReadWriteItem<bool> OnUsb = new ReadWriteItem<bool>("video:video_on_usb");
        public readonly ReadWriteItem<int> FileIndex = new ReadWriteItem<int>("video:video_file_index");
    }
}