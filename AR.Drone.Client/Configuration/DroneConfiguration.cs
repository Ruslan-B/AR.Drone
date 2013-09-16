using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using AR.Drone.Client.Commands;

namespace AR.Drone.Client.Configuration
{
    public class DroneConfiguration
    {
        private static readonly string DefaultApplicationId = "default";
        private static readonly string DefaultUserId = "default";

        private static string NewSessionId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        private static readonly Regex _reKeyValue = new Regex(@"(?<key>\w+:\w+) = (?<value>.*)");


        private readonly string _applicationId;
        private readonly string _userId;
        private readonly string _sessionId;

        private readonly Dictionary<string, string> _items;
        private readonly HashSet<string> _changed;


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

        public DroneConfiguration(string applicationId, string userId, string sessionId)
        {
            _applicationId = applicationId;
            _userId = userId;
            _sessionId = sessionId;

            _items = new Dictionary<string, string>();
            _changed = new HashSet<string>();

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
        
        public DroneConfiguration(string application, string user)
            : this(application, user, NewSessionId())
        {
        }

        public DroneConfiguration(string application)
            : this(application, DefaultUserId, NewSessionId())
        {
        }

        public DroneConfiguration()
            : this(DefaultApplicationId, DefaultUserId, NewSessionId())
        {
        }
        
        protected internal Dictionary<string, string> Items
        {
            get { return _items; }
        }

        protected internal HashSet<string> Changed
        {
            get { return _changed; }
        }

        public void SendChanges(DroneClient client)
        {
            foreach(var key in _changed) 
            {
                client.Send(new ConfigIdsCommand(_sessionId, _userId, _applicationId));
                client.Send(new ConfigCommand(key, _items[key]));
            }
            _changed.Clear();
        }

        public static DroneConfiguration Parse(string input)
        {
            DroneConfiguration configuration = new DroneConfiguration();

            MatchCollection matches = _reKeyValue.Matches(input);
            foreach(Match match in matches)
            {
                string key = match.Groups["key"].Value;
                string value = match.Groups["value"].Value;
                configuration._items.Add(key, value);
            }
            return configuration;
        }
    }
}