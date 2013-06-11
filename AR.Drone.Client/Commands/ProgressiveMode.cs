using System;

namespace AR.Drone.Client.Commands
{
    [Flags]
    public enum ProgressiveMode
    {
        None = 0,
        Progressive = 1 << 0,
        CombinedYaw = 1 << 2,
        AbsoluteControl = 1 << 3
    }
}