using System;

namespace AR.Drone.Client.Commands
{
    [Flags]
    public enum FlightMode
    {
        /// <summary>
        /// The hover.
        /// </summary>
        Hover = 0,
        /// <summary>
        /// The progressive is an enabling bit.
        /// </summary>
        Progressive = 1 << 0,
        /// <summary>
        /// The combined yaw. Usage: FlightMode.Progressive | FlightMode.CombinedYaw
        /// </summary>
        CombinedYaw = 1 << 2,
        /// <summary>
        /// The absolute control. Usage: FlightMode.Progressive | FlightMode.AbsoluteControl
        /// </summary>
        AbsoluteControl = 1 << 3
    }
}