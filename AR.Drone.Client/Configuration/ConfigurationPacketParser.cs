using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using AR.Drone.Data;

namespace AR.Drone.Client.Configuration
{
    public class ConfigurationPacketParser
    {
        private static readonly Regex _reKeyValue = new Regex(@"(?<key>\w+:\w+) = (?<value>.*)");

        public static bool TryUpdate(DroneConfiguration configuration, ConfigurationPacket packet)
        {
            bool updated = false;
            using (var ms = new MemoryStream(packet.Data))
            using (var sr = new StreamReader(ms))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Match match = _reKeyValue.Match(line);
                    if (match.Success)
                    {
                        string key = match.Groups["key"].Value;
                        IConfigurationItem item;
                        if (configuration.Items.TryGetValue(key, out item))
                        {
                            string value = match.Groups["value"].Value;
                            if (item.TryUpdate(value))
                            {
                                updated = true;
                            }
                        }
                        else
                        {
                            Trace.TraceWarning("Configuration key {0} is not supported by parser. Original line: {1}", key, line);
                        }
                    }
                }
            }
            return updated;
        }
    }
}