using AR.Drone.Client.Navigation.Native;
using AR.Drone.Client.Packets;

namespace AR.Drone.Client.Navigation
{
    public class NavigationPacketParser
    {
        public static bool TryParse(ref NavigationPacket packet, out NavigationData navigationData)
        {
            navigationData = new NavigationData();
            NavdataBag navdataBag;
            if (NavdataBagParser.TryParse(ref packet, out navdataBag))
            {
                navigationData = NavdataConverter.ToNavigationData(navdataBag);
                return true;
            }
            return false;
        }
    }
}