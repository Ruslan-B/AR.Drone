using System;

namespace AR.Drone.Data.Navigation
{
    [Flags]
    public enum NavigationState : byte
    {
        Unknown = 0,
        Landed = 1 << 0,
        Flying = 1 << 1,
        Takeoff = 1 << 2,
        Landing = 1 << 3,
        Hovering = 1 << 4,
        Command = 1 << 5,
        Control = 1 << 6,
        Emergency = 1 << 7
    }
}