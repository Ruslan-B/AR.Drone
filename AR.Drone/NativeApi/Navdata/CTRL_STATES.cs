using System;

namespace AR.Drone.NativeApi
{
    [Flags]
    public enum CTRL_STATES : uint
    {
        CTRL_DEFAULT,
        //CTRL_INIT,
        CTRL_LANDED = 2 << 16,
        CTRL_FLYING = 4 << 16,
        CTRL_HOVERING = CTRL_FLYING,
        //CTRL_TEST,
        CTRL_TRANS_TAKEOFF = CTRL_LANDED | CTRL_FLYING,
        CTRL_TRANS_GOTOFIX = CTRL_FLYING | 1,
        CTRL_TRANS_LANDING = 8 << 16
    }
}