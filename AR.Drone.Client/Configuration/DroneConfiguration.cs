using System;
using System.Collections.Generic;
using System.Linq;
using AR.Drone.Client.Configuration.Sections;

namespace AR.Drone.Client.Configuration
{
    public class DroneConfiguration : INetworkConfiguration
    {
        public readonly ControlSection Control;
        public readonly CustomSection Custom;
        public readonly DetectSection Detect;
        public readonly GeneralSection General;
        public readonly GpsSection Gps;
        public readonly LedsSection Leds;
        public readonly NetworkSection Network;
        public readonly PicSection Pic;
        public readonly SyslogSection Syslog;
        public readonly UserboxSection Userbox;
        public readonly VideoSection Video;

        private readonly Dictionary<string, IConfigurationItem> _items;

        public DroneConfiguration()
        {
            DroneHostname = "192.168.1.1";

            General = new GeneralSection();
            Control = new ControlSection();
            Network = new NetworkSection();
            Pic = new PicSection();
            Video = new VideoSection();
            Leds = new LedsSection();
            Detect = new DetectSection();
            Syslog = new SyslogSection();
            Userbox = new UserboxSection();
            Gps = new GpsSection();
            Custom = new CustomSection();

            _items = GetItems(General)
                .Concat(GetItems(Control))
                .Concat(GetItems(Network))
                .Concat(GetItems(Pic))
                .Concat(GetItems(Video))
                .Concat(GetItems(Leds))
                .Concat(GetItems(Detect))
                .Concat(GetItems(Syslog))
                .Concat(GetItems(Userbox))
                .Concat(GetItems(Gps))
                .Concat(GetItems(Custom))
                .ToDictionary(x => x.Key);
        }

        public Dictionary<string, IConfigurationItem> Items
        {
            get { return _items; }
        }

        public string DroneHostname { get; set; }

        private static IEnumerable<IConfigurationItem> GetItems(object section)
        {
            Type type = section.GetType();
            IEnumerable<IConfigurationItem> items =
                type.GetFields()
                    .Where(x => typeof (IConfigurationItem).IsAssignableFrom(x.FieldType))
                    .Select(x => (IConfigurationItem) x.GetValue(section));
            return items;
        }
    }
}