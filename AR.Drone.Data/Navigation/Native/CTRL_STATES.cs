namespace AR.Drone.Data.Navigation.Native
{
    public enum CTRL_STATES : ushort
    {
        CTRL_DEFAULT,
        CTRL_INIT,
        CTRL_LANDED,
        CTRL_FLYING,
        CTRL_HOVERING,
        CTRL_TEST,
        CTRL_TRANS_TAKEOFF,
        CTRL_TRANS_GOTOFIX,
        CTRL_TRANS_LANDING,
        CTRL_TRANS_LOOPING
    }
}