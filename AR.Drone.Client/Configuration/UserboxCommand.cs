using System;
using System.Globalization;

namespace AR.Drone.Client.Configuration
{
    public struct UserboxCommand
    {
        private const string DateTimeFormat = "yyyyMMdd_HHmmss";

        public UserboxCommand(UserboxCommandType type, DateTime timestamp) 
            : this()
        {
            Type = type;
            Timestamp = timestamp;
        }

        public UserboxCommand(UserboxCommandType type) 
            : this (type, DateTime.Now)
        {
        }

        public UserboxCommandType Type { get; private set; }
        public DateTime Timestamp { get; private set; }

        public static UserboxCommand Parse(string value)
        {
            string[] parts = value.Split(',');
            var command = new UserboxCommand();
            UserboxCommandType type;
            if (parts.Length > 0 && Enum.TryParse(parts[0], out type)) command.Type = type;
            DateTime timestamp;
            switch (type)
            {
                case UserboxCommandType.Start:
                    if (parts.Length > 1 && DateTime.TryParseExact(parts[1], DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out timestamp))
                        command.Timestamp = timestamp;
                    break;
                case UserboxCommandType.Screenshot:
                    if (parts.Length > 3 && DateTime.TryParseExact(parts[3], DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out timestamp))
                        command.Timestamp = timestamp;
                    break;
            }
            return command;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case UserboxCommandType.Screenshot: 
                    return string.Format("{0},1,10,{1}", (int) Type, Timestamp.ToString(DateTimeFormat));
                case UserboxCommandType.Start: 
                    return string.Format("{0},{1}", (int) Type, Timestamp.ToString(DateTimeFormat));
                default:
                    return string.Format("{0}", (int) Type);
            }
        }
    }
}