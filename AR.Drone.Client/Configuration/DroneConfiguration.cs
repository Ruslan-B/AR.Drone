using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using AR.Drone.Client.Commands;

namespace AR.Drone.Client.Configuration
{
    public class DroneConfiguration
    {
        private static readonly Regex ReKeyValue = new Regex(@"(?<key>\w+:\w+) = (?<value>.*)");

        private readonly Dictionary<string, string> _items;
        private readonly ConcurrentQueue<string> _changed;
        public readonly GeneralSection General;
        public readonly ControlSection Control;
        public readonly NetworkSection Network;
        public readonly PicSection Pic;
        public readonly VideoSection Video;
        public readonly LedsSection Leds;
        public readonly DetectSection Detect;
        public readonly SyslogSection Syslog;
        public readonly UserboxSection Userbox;
        public readonly GpsSection Gps;
        public readonly CustomSection Custom;

        public DroneConfiguration()
        {
            _items = new Dictionary<string, string>();
            _changed = new ConcurrentQueue<string>();

            General = new GeneralSection(this);
            Control = new ControlSection(this);
            Network = new NetworkSection(this);
            Pic = new PicSection(this);
            Video = new VideoSection(this);
            Leds = new LedsSection(this);
            Detect = new DetectSection(this);
            Syslog = new SyslogSection(this);
            Userbox = new UserboxSection(this);
            Gps = new GpsSection(this);
            Custom = new CustomSection(this);
        }

        public Dictionary<string, string> Items
        {
            get { return _items; }
        }

        protected internal ConcurrentQueue<string> Changed
        {
            get { return _changed; }
        }

        public void SendChanges(DroneClient client)
        {
            client.Send(new ControlCommand(ControlMode.AckControlMode));

            string key;
            while (_changed.TryDequeue(out key))
            {
                if (Custom.SessionId != null && Custom.ProfileId != null && Custom.ApplicationId != null)
                {
                    client.Send(new ConfigIdsCommand(Custom.SessionId, Custom.ProfileId, Custom.ApplicationId));
                }

                client.Send(new ConfigCommand(key, _items[key]));
            }
        }

        public static DroneConfiguration Parse(string input)
        {
            var configuration = new DroneConfiguration();

            MatchCollection matches = ReKeyValue.Matches(input);
            foreach (Match match in matches)
            {
                string key = match.Groups["key"].Value;
                string value = match.Groups["value"].Value;
                configuration._items.Add(key, value);
            }
            return configuration;
        }

        public static string NewId()
        {
            return Guid.NewGuid().ToString("N").Substring(0,8);
        }
    }
}