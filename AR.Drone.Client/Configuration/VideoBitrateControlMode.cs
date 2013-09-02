namespace AR.Drone.Client.Configuration
{
    public enum VideoBitrateControlMode
    {
        Disabled = 0, // Bitrate set to video:max_bitrate
        Dynamic, // Video bitrate varies in [250;video:max_bitrate] kbps
        Manual // Video stream bitrate is fixed by the video:bitrate key
    }
}