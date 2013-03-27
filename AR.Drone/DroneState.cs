using System;

namespace AR.Drone
{
    [Flags]
    public enum DroneState
    {
        Unknown = 0,
        Landed = 1 << 1,
        Flying = 1 << 2,
        Landing = 1 << 3,
        Takeoff = 1 << 5,
        Hovering = 1 << 6,
        Progress = 1 << 7,
        BatteryLow = 1 << 8,
        Emergency = 1 << 9,
    }
}