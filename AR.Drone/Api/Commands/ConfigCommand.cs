using System;

namespace AR.Drone.Api.Commands
{
    public class ConfigCommand : IATCommand
    {
        private readonly string _key;
        private readonly string _value;

        public ConfigCommand(string key, bool value)
            : this(key, value.ToString().ToUpper())
        {
        }

        public ConfigCommand(string key, int value)
            : this(key, value.ToString("D"))
        {
        }

        public ConfigCommand(string key, Enum value)
            : this(key, value.ToString("D"))
        {
        }

        public ConfigCommand(string key, string value)
        {
            _key = key;
            _value = value;
        }


        public string ToAt(int sequenceNumber)
        {
            return string.Format("AT*CONFIG={0},\"{1}\",\"{2}\"\r", sequenceNumber, _key, _value);
        }
    }
}