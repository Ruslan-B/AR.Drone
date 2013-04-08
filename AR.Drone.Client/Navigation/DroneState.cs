using System;

namespace AR.Drone.Client.Navigation
{
    [Flags]
    public enum DroneState : byte
    {
        Unknown = 0,
        Landed = 1 << 1,
        Flying = 1 << 2,
        Takeoff = 1 << 3,
        Landing = 1 << 4,
        Hovering = 1 << 5,
        Emergency = 1 << 6,
    }
}