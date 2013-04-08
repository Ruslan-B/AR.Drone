namespace AR.Drone.Client.Command
{
    public enum RefMode
    {
        Default = 1 << 18 | 1 << 20 | 1 << 22 | 1 << 24 | 1 << 28,
        Land = (0 << 9) | Default,
        Takeoff = (1 << 9) | Default,
        Emergency = (1 << 8) | Default
    }
}