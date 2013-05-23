namespace AR.Drone.Client.Configuration.Sections
{
    public enum VideoChannelType
    {
        Horizontal = 0, // Selects the horizontal camera
        Vertical, // Selects the vertical camera
        Next = 4 // Selects the next available format among those above.
    }
}