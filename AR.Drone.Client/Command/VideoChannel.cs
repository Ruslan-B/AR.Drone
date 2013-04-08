namespace AR.Drone.Client.Command
{
    public enum VideoChannel
    {
        First = 0, // First element
        Horizontal = First, // Selects the horizontal camera
        Vertical, // Selects the vertical camera
        HorizontalAndSmallVertical, // Selects the horizontal camera with vertical camera picture inserted in the left-top corner
        VerticalAndSmallHorizontal, // Selects the vertical camera with horizontal camera picture inserted in the left-top corner
        Last = VerticalAndSmallHorizontal, // Last element
        Next // Selects the next available format among those above.
    }
}