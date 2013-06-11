using System;

namespace AR.Drone.Client.Commands
{
    [Flags]
    public enum FlightMode
    {
        Hover = 0,
        Progressive = 1 << 0,
        CombinedYaw = 1 << 2,
        AbsoluteControl = 1 << 3
    }
}