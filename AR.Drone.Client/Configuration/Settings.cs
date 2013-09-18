using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AR.Drone.Client.Configuration
{
    public class Settings
    {
        private static readonly Regex ReKeyValue = new Regex(@"(?<key>\w+:\w+) = (?<value>.*)");

        private readonly Dictionary<string, string> _items;
        private readonly ConcurrentQueue<KeyValuePair<string,string>> _changes;
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

        public Settings()
        {
            _items = new Dictionary<string, string>();
            _changes = new ConcurrentQueue<KeyValuePair<string,string>>();

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

        public ConcurrentQueue<KeyValuePair<string,string>> Changes
        {
            get { return _changes; }
        }

        public static Settings Parse(string input)
        {
            var configuration = new Settings();

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