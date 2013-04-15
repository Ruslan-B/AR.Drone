using System.Runtime.InteropServices;

namespace AR.Drone.Client.Configuration.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public class CustomSection
    {
        public readonly ReadOnlyItem<string> ApplicationId = new ReadOnlyItem<string>("custom:application_id");
        public readonly ReadOnlyItem<string> ApplicationDescription = new ReadOnlyItem<string>("custom:application_desc");
        public readonly ReadOnlyItem<string> ProfileId = new ReadOnlyItem<string>("custom:profile_id");
        public readonly ReadOnlyItem<string> ProfileDescription = new ReadOnlyItem<string>("custom:profile_desc");
        public readonly ReadOnlyItem<string> SessionId = new ReadOnlyItem<string>("custom:session_id");
        public readonly ReadOnlyItem<string> SessionDescription = new ReadOnlyItem<string>("custom:session_desc");
    }
}