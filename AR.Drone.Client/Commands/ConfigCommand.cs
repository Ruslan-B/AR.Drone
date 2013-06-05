using System;
using AR.Drone.Data.Helpers;

namespace AR.Drone.Client.Commands
{
    public class ConfigCommand : ATCommand
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

        public ConfigCommand(string key, float value)
            : this(key, ConversionHelper.ToInt(value))
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


        protected override string ToAt(int sequenceNumber)
        {
            return string.Format("AT*CONFIG={0},\"{1}\",\"{2}\"\r", sequenceNumber, _key, _value);
        }
    }
}