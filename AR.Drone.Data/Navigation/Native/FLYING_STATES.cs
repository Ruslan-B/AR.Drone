namespace AR.Drone.Data.Navigation.Native
{
    public enum FLYING_STATES : ushort
    {
        FLYING_OK,
        FLYING_LOST_ALT,
        FLYING_LOST_ALT_GO_DOWN,
        FLYING_ALT_OUT_ZONE,
        FLYING_COMBINED_YAW,
        FLYING_BRAKE,
        FLYING_NO_VISION,
    }
}