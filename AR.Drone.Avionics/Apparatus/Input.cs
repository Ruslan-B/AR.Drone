using AR.Drone.Client;

namespace AR.Drone.Avionics.Apparatus
{
    /// <summary>
    /// The structure represents an input that is to be sent to a DroneClient
    /// </summary>
    public struct Input
    {
        /// <summary>
        /// Supported commands
        /// </summary>
        public enum Type { Progress, Takeoff, Hover, Land, Emergency, ResetEmergency, FlatTrim };

        /// <summary>
        /// Value limits for commands that can be sent to a drone
        /// </summary>
        public static class Limits
        {
            public static class Roll
            {
                public const float Max = 1.0f;
                public const float Min = -1.0f;
            }

            public static class Pitch
            {
                public const float Max = 1.0f;
                public const float Min = -1.0f;
            }

            public static class Yaw
            {
                public const float Max = 1.0f;
                public const float Min = -1.0f;
            }

            public static class Gaz
            {
                public const float Max = 1.0f;
                public const float Min = -1.0f;
            }
        }

        /// <summary>
        /// Command type to be sent
        /// </summary>
        public Type Command;

        /// <summary>
        /// Type of progress mode, considered only for the Type.Progress command
        /// </summary>
        public Client.Command.FlightMode ProgressMode;

        /// <summary>
        /// Set roll, pitch, yaw and gaz for the Type.Progress command
        /// </summary>
        public float Roll;
        public float Pitch;
        public float Yaw;
        public float Gaz;

        /// <summary>
        /// Resets the structure into a zero progress mode (Hover)
        /// </summary>
        public void Reset() {
            Command = Type.Progress;
            ProgressMode = Client.Command.FlightMode.Progressive;
            Roll = Pitch = Yaw = Gaz = 0.0f;
        }

        /// <summary>
        /// Sends the command with its parameters to a provided DroneClient
        /// </summary>
        /// <param name="aDroneClient">Drone client to the the command to</param>
        public void Send(DroneClient aDroneClient)
        {
            switch (Command)
            {
                case Type.Progress: aDroneClient.Progress(ProgressMode, roll: Roll, pitch: Pitch, yaw: Yaw, gaz: Gaz); break;
                case Type.Takeoff: aDroneClient.Takeoff(); break;
                case Type.Hover: aDroneClient.Hover(); break;
                case Type.Land: aDroneClient.Land(); break;
                case Type.Emergency: aDroneClient.Emergency(); break;
                case Type.ResetEmergency: aDroneClient.ResetEmergency(); break;
                case Type.FlatTrim: aDroneClient.FlatTrim(); break;
            }
        }
    }
}
