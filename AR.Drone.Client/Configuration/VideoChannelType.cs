namespace AR.Drone.Client.Configuration
{
    public enum VideoChannelType
    {
        Horizontal = 0, // Selects the horizontal camera
        Vertical = 1, // Selects the vertical camera
        HorizontalPlusSmallVertical = 2, // AR.Drone 1.0 only 
        VerticalPlusSmallHorizontal = 3, // AR.Drone 1.0 only
        Next = 4 // Selects the next available format among those above.
    }
}