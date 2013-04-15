using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class NetworkSection
    {
        public readonly ReadWriteItem<string> SsidSinglePlayer = new ReadWriteItem<string>("network:ssid_single_player");
        public readonly ReadWriteItem<string> SsidMultiPlayer = new ReadWriteItem<string>("network:ssid_multi_player");
        public readonly ReadWriteItem<int> WifiMode = new ReadWriteItem<int>("network:wifi_mode");
        public readonly ReadWriteItem<int> WifiRate = new ReadWriteItem<int>("network:wifi_rate");
        public readonly ReadWriteItem<string> OwnerMac = new ReadWriteItem<string>("network:owner_mac");
    }
}