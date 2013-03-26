using System;

namespace AR.Drone
{
    [Flags]
    public enum DroneControllerState
    {
        None = 0,
        Landed = 1 << 2,
        Flying = 1 << 3,
        Landing = 1 << 4,
        Takeoff = 1 << 5,
        Emergency = 1 << 6,
        //Hovering = ??,
    }
}