using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class GpsSection
    {
        public readonly ReadOnlyItem<double> Latitude = new ReadOnlyItem<double>("gps:latitude");
        public readonly ReadOnlyItem<double> Longitude = new ReadOnlyItem<double>("gps:longitude");
        public readonly ReadOnlyItem<double> Altitude = new ReadOnlyItem<double>("gps:altitude");
    }
}