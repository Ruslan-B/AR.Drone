using System;

namespace AR.Drone.Data.Navigation
{
    [Flags]
    public enum NavigationState : ushort
    {
        Unknown = 0,
        Landed = 1 << 1,
        Flying = 1 << 2,
        Takeoff = 1 << 3,
        Landing = 1 << 4,
        Hovering = 1 << 5,
        Command = 1 << 6,
        Control = 1 << 7,
        Emergency = 1 << 8
    }
}