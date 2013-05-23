using AR.Drone.Client.Acquisition.Navigation.Native;
using AR.Drone.Data;

namespace AR.Drone.Client.Acquisition.Navigation
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