using System;
using System.Globalization;

namespace AR.Drone.Client.Configuration
{
    public struct UserboxCommand
    {
        public const string DateTimeFormat = "yyyyMMdd_HHmmss";

        public UserboxCommand(UserboxCommandType type, int delay, int number, DateTime timestamp)
            : this()
        {
            Type = type;
            Delay = delay;
            Number = number;
            Timestamp = timestamp;
        }

        public UserboxCommand(UserboxCommandType type, DateTime timestamp)
            : this()
        {
            Type = type;
            Timestamp = timestamp;
        }

        public UserboxCommand(UserboxCommandType type)
            : this()
        {
            Type = type;
            Timestamp = DateTime.Now;
        }

        /// <summary> Type of userbox command. </summary>
        public UserboxCommandType Type { get; private set; }

        /// <summary> The number of screenshots to take. </summary>
        public int Number { get; private set; }

        /// <summary> Delay between screenshots in seconds. </summary>
        public int Delay { get; private set; }

        /// <summary> Timestamp. </summary>
        public DateTime Timestamp { get; private set; }

        public static UserboxCommand Parse(string value)
        {
            string[] parts = value.Split(',');
            var command = new UserboxCommand();
            var type = UserboxCommandType.Stop;
            if (parts.Length > 0 && Enum.TryParse(parts[0], out type)) command.Type = type;
            DateTime timestamp;
            switch (type)
            {
                case UserboxCommandType.Start:
                    if (parts.Length > 1)
                    {
                        if (TryParseDate(parts[1], out timestamp)) command.Timestamp = timestamp;
                    }
                    break;
                case UserboxCommandType.Screenshot:
                    if (parts.Length > 3)
                    {
                        if (TryParseDate(parts[3], out timestamp)) command.Timestamp = timestamp;
                    }
                    break;
            }
            return command;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case UserboxCommandType.Screenshot:
                    return string.Format("{0},{1},{2},{3}", (int) Type, Delay, Number, Timestamp.ToString(DateTimeFormat));
                case UserboxCommandType.Start:
                    return string.Format("{0},{1}", (int) Type, Timestamp.ToString(DateTimeFormat));
                default:
                    return string.Format("{0}", (int) Type);
            }
        }

        private static bool TryParseDate(string s, out DateTime timestamp)
        {
            return DateTime.TryParseExact(s, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out timestamp);
        }
    }
}