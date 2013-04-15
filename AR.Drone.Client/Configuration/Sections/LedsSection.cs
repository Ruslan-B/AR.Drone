using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class LedsSection
    {
        public readonly ReadWriteItem<string> Animation = new ReadWriteItem<string>("leds:leds_anim");
    }
}